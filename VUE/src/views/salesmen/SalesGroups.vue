<template>
  <!-- 销售员分组组件 -->
  <div>
    <!-- 面包屑区域 -->
    <BreadCrumb firstname="销售员" secondname="销售员分组"></BreadCrumb>
    <!-- 分组信息区域 -->
    <div class="group-content" v-for="item in groupList" :key="item.groupId" @click="showGroupInfo(item.groupId)">
      <div class="img"></div>
      <div class="info">
        <div class="first">{{ item.groupName }}</div>
        <div class="first second">负责人: {{ item.groupManager }}</div>
        <!-- <div class="first second">组员：李四、王五...</div> -->
      </div>
      <div class="operate">
        <ul>
          <li><el-button type="warning" size="medium" round @click.stop="editDialogShow(item.groupId)">修改信息</el-button></li>
          <li><el-button v-if="item.groupId !== 4" type="danger" size="medium" round @click.stop="deleteGroupInfo(item.groupId)">删除分组</el-button></li>
        </ul>
      </div>
    </div>
    <!-- 查看分组信息弹窗 -->
    <el-dialog title="分组信息" :visible.sync="infoDialogVisible" center width="60%" @close="groupInfoDialogClosed">
      <el-descriptions title="基础信息" :column="3" size="medium" border>
        <el-descriptions-item>
          <template slot="label">
            <i class="el-icon-collection-tag"></i>
            Id
          </template>
          {{ groupInfo.groupId }}
        </el-descriptions-item>
        <el-descriptions-item>
          <template slot="label">
            <i class="el-icon-more-outline"></i>
            组名
          </template>
          {{ groupInfo.groupName }}
        </el-descriptions-item>
        <el-descriptions-item>
          <template slot="label">
            <i class="el-icon-user"></i>
            负责人
          </template>
          {{ groupInfo.groupManager }}
        </el-descriptions-item>
      </el-descriptions>
      <el-descriptions title="成员信息" size="medium" style="margin-top: 45px">
        <template v-if="groupInfo.groupId !== 4" slot="extra">
          <el-button v-if="insertVisible" type="warning" icon="el-icon-circle-plus-outline" size="small" @click="showNonGroupSales">添加成员</el-button>
          <el-button v-else type="warning" size="small" icon="el-icon-setting" @click="editGroupMember">管理成员</el-button>
          <el-button v-if="insertVisible" type="primary" size="small" icon="el-icon-finished" @click="confirmChange">确定</el-button>
        </template>
      </el-descriptions>
      <el-table :data="groupMemberList" border stripe>
        <el-table-column prop="salesManId" label="#"></el-table-column>
        <el-table-column prop="salesManName" label="姓名"></el-table-column>
        <!-- 操作 -->
        <el-table-column v-if="insertVisible" fixed="right" label="操作">
          <template slot-scope="scope">
            <!-- 设置权限 -->
            <el-button @click="deleteGroupMember(scope.row.salesManId)" type="success" icon="el-icon-s-flag" size="small">设为组长</el-button>
            <!-- 移出分组 -->
            <el-button @click="deleteGroupMember(scope.row.salesManId)" type="danger" icon="el-icon-delete" size="small">移除</el-button>
          </template>
        </el-table-column>
      </el-table>
      <!-- 添加成员Dialog -->
      <el-dialog width="20%" title="选择销售员" center :visible.sync="innerVisible" append-to-body>
        <el-table stripe border :data="nonGroupList" @selection-change="handleSelectionChange">
          <el-table-column type="selection" width="55"> </el-table-column>
          <el-table-column prop="salesManName" label="姓名"></el-table-column>
        </el-table>
        <span slot="footer" class="dialog-footer">
          <el-button @click="innerVisible = false">取 消</el-button>
          <el-button type="primary" @click="addGroupMemmber">确 定</el-button>
        </span>
      </el-dialog>
    </el-dialog>
    <!-- 修改分组信息弹窗 -->
    <el-dialog title="修改信息" :visible.sync="editDialogVisible" width="50%" @close="editDialogClosed">
      <el-form :model="editGroupForm" :rules="groupFormRules" ref="editFormRef" label-width="60px">
        <!-- 名称 -->
        <el-form-item label="名称" prop="groupName">
          <el-input v-model="editGroupForm.groupName"></el-input>
        </el-form-item>
        <el-form-item label="负责人" prop="groupManager">
          <el-input v-model="editGroupForm.groupManager"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editGroupInfo">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import BreadCrumb from '@/components/subcomponents/BreadCrumb.vue'
import request from '@/utils/request.js'
import { Loading } from 'element-ui'

export default {
  name: 'MySalesGroups',
  components: {
    BreadCrumb
  },
  data() {
    return {
      groupList: [],
      infoDialogVisible: false,
      groupInfo: {},
      salesManList: [],
      groupMemberList: [],
      roleChange: false,
      editDialogVisible: false,
      editGroupForm: '',
      groupFormRules: {},
      preAddMemberGroup: 4,
      innerVisible: false,
      nonGroupList: [], // 没有分组的成员列表
      selectedMember: [],
      preJoinGroupMember: { // 准备进组的成员
        salesManId: 0,
        salesManName: '',
        salesManPwd: '',
        salesManPhone: '',
        salesManEmail: '',
        salesManImg: '',
        groupId: 4,
        groupName: ''
      },
      preDeleteFromGroupMember: {}, // 准备出组的成员
      insertVisible: false
    }
  },
  created() {
    this.getGroupList()
  },
  methods: {
    // 调用接口获取数据
    async getGroupList() {
      const { data: res } = await request.get('Salesgroups')
      if (res.status === 500) return this.$message.info('没有分组信息')
      if (res.status !== 200) return this.$message.error('获取分组信息失败')
      this.groupList = res.data
    },
    // 显示分组详细信息
    async showGroupInfo(id) {
      this.preAddMemberGroup = id
      const { data: res } = await request.get('SalesGroups/' + id)
      if (res.status !== 200) return this.$message.error('获取分组信息失败')
      this.groupInfo = res.data
      const { data: sale } = await request.get('Salesmen')
      this.salesManList = sale.data
      this.groupMemberList = this.salesManList.filter(item => item.groupId === id)
      this.infoDialogVisible = true
    },
    // 监听管理成员按钮点击事件
    editGroupMember() {
      this.insertVisible = true
    },
    // 监听添加成员按钮点击事件
    async showNonGroupSales() {
      this.innerVisible = true
      const { data: res } = await request.get('Salesmen')
      if (res.status !== 200) return this.$message.error('获取销售员列表失败')
      this.nonGroupList = res.data.filter(item => item.groupId === 4)
    },
    // 监听成员列表选择事件
    handleSelectionChange(val) {
      // 将选中的数据保存到 selectedMember 中
      this.selectedMember = val
    },
    // 监听添加成员确定按钮点击事件
    async addGroupMemmber() {
      if (this.selectedMember.length <= 0) return this.$message.info('请先选择成员')
      const confirmText = await this.$confirm('确定添加以上成员?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消添加')
      let loadingInstance = Loading.service({
        text: '玩命加载中',
        spinner: 'el-icon-loading'
      })
      for (let index = 0; index < this.selectedMember.length; index++) {
        // 调用修改销售员组号接口
        const { data: res } = await request.get('Salesmen/' + this.selectedMember[index].salesManId)
        this.res.data.groupId = this.preAddMemberGroup
        const { data: res1 } = await request.put('Salesmen/' + res.data.salesManId, res.data)
        if (res1.status !== 200) {
          loadingInstance.close()
          return this.$message.error(`添加第${++index}个成员时失败`)
        }
      }
      loadingInstance.close()
      this.innerVisible = false
      this.getGroupList()
      this.$message.success('添加组员成功!')
    },
    // 监听角色设置完成后确定按钮点击事件
    confirmChange() {
      this.insertVisible = false
    },
    // 监听分组信息 Dialog 关闭事件
    groupInfoDialogClosed() {
      this.insertVisible = false
    },
    // 监听移除按钮点击事件
    async deleteGroupMember(id) {
      const confirmText = await this.$confirm('此操作将会把该销售员移除该组, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消操作')
      const { data: res } = await request.get('Salesmen/id/' + id)
      this.preDeleteFromGroupMember = res.data[0]
      this.preDeleteFromGroupMember.groupId = 4
      const { data: res1 } = await request.put('Salesmen/' + id, this.preDeleteFromGroupMember)
      if (res1.status !== 200) return this.$message.error('移除组员失败')
      this.showGroupInfo(this.groupInfo.groupId)
    },
    // 监听修改信息按钮点击事件
    async editDialogShow(id) {
      const { data: res } = await request.get('SalesGroups/' + id)
      if (res.status !== 200) return this.$message.error('获取信息失败')
      this.editGroupForm = res.data
      this.editDialogVisible = true
    },
    // 监听修改信息框关闭事件
    editDialogClosed() {
      this.$refs.editFormRef.resetFields()
    },
    // 监听修改确定按钮点击事件
    async editGroupInfo() {
      const { data: res } = await request.put('SalesGroups/' + this.editGroupForm.groupId, this.editGroupForm)
      if (res.status !== 200) return this.$message.error('修改信息失败')
      this.editDialogVisible = false
      this.getGroupList()
      this.$message.success('修改信息成功')
    },
    // 监听删除按钮点击事件
    async deleteGroupInfo(id) {
      if (this.salesManList.filter(item => item.groupId === id).length > 0) return this.$message.info('组内存在组员，不能删除该组，如要删除，请先将组内组员移除')
      const confirmText = await this.$confirm('此操作将会永久删除该组, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消操作')
      const { data: res } = await request.delete('SalesGroups/' + id)
      if (res.status !== 200) return this.$message.error('删除分组失败')
      this.getGroupList()
      this.$message.success('删除分组信息成功')
    }
  }
}
</script>

<style lang="less" scoped>
.group-content {
  width: 420px;
  height: 150px;
  margin-top: 20px;
  margin-left: 150px;
  background-color: #ecf5ff;
  border-radius: 10px;
  float: left;
  display: flex;
  font-family: 'Helvetica Neue', Helvetica, 'PingFang SC', 'Hiragino Sans GB', 'Microsoft YaHei', '微软雅黑', Arial, sans-serif;
}

.img {
  width: 10%;
  height: 100%;
  background-color: #409eff;
  border-radius: 10px 0 0 10px;
  cursor: pointer;
}

.info {
  width: 65%;
  height: 100%;
  background-color: #d9ecff;
  padding: 15px;
  box-sizing: border-box;
  cursor: pointer;
}

.id {
  width: 100%;
  line-height: 30px;
  font-size: 25px;
}

.first {
  width: 100%;
  line-height: 30px;
  font-size: 25px;
}

.second {
  margin-top: 10px;
  font-size: 18px;
  color: #666;
}

.operate {
  width: 25%;
  height: 100%;
  border-radius: 0 10px 10px 0;
  text-align: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.operate ul {
  list-style-type: none;
  height: 90px;
}

.operate ul li {
  text-align: center;
  list-style: none;
  display: block;
  line-height: 45px;
}

.group-content:hover {
  box-shadow: 1px 2px 5px #aaa;
}
</style>
