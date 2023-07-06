<template>
<!--行动记录模块-->
  <div class="actionnotes-container">
    <!-- 面包屑区域 -->
    <BreadCrumb firstname="客户关系" secondname="行动记录"></BreadCrumb>
    <!-- 搜索区域 -->
    <el-card style="margin-bottom: 15px">
      <el-row :gutter="20">
        <!-- 行动计划名输入框 -->
        <el-col :span="6">
          <el-input v-model="querylist.salesManId" placeholder="销售员 id" clearable @clear="clearRecord"></el-input>
        </el-col>
        <!-- 查询按钮 -->
        <el-col :span="16">
          <el-button @click="findRecordById" type="primary">查询</el-button>
        </el-col>
        <!-- 添加按钮 -->
        <el-col :span="1">
          <el-button @click="addDialogVisible = true" type="primary">添加</el-button>
        </el-col>
      </el-row>
    </el-card>
    <!-- 行动计划列表区域 -->
    <el-card>
      <!-- 行动计划列表 -->
      <el-table :data="recordlist">
        <!-- 销售员编号 -->
        <el-table-column prop="salesManId" label="销售员编号" sortable> </el-table-column>
         <!-- 客户编号 -->
        <el-table-column prop="customerId" label="客户编号" sortable> </el-table-column>
        <!-- 开始时间 -->
        <el-table-column prop="recordDate" label="记录时间">
          <template slot-scope="scope">
            {{ scope.row.recordDate | initDate }}
          </template>
        </el-table-column>
        <!-- 计划内容 -->
        <el-table-column prop="recordInfo" label="行动内容">
          <!-- 将内容进行渲染，限制内容的长度，多余部分用省略号代替 -->
          <template slot-scope="scope">
            <div class="line-limit-length" style="max-width: 180px">{{ scope.row.recordInfo }}</div>
          </template>
        </el-table-column>
        <!-- 记录方式 -->
        <el-table-column prop="recordWay" label="记录方式" sortable>
          <template slot-scope="scope">
            {{ scope.row.recordInfo === 0 ? '手抄本' : scope.row.recordInfo === 1 ? '日志' : '报表' }}
          </template>
        </el-table-column>
        <!-- 操作 -->
        <el-table-column fixed="right" label="操作" width="180">
          <template slot-scope="scope">
            <!-- 查看信息 -->
            <el-button @click="recordInfoDialogShow(scope.row.recordId)" type="primary" icon="el-icon-view" size="small"></el-button>
            <!-- 编辑信息 -->
            <el-button @click="editRecordDialogShow(scope.row.recordId)" type="warning" icon="el-icon-edit" size="small"></el-button>
            <!-- 删除信息 -->
            <el-button @click="deleteRecordInfo(scope.row.recordId)" type="danger" icon="el-icon-delete" size="small"></el-button>
          </template>
        </el-table-column>
      </el-table>
      <!-- 分页区域 -->
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="currentPage"
        :page-size="pagesize"
        :page-sizes="[6, 10, 15]"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </el-card>
    <!-- 查看行动信息弹窗 -->
    <el-dialog title="行动信息" :visible.sync="infoDialogVisible" width="50%">
      <el-descriptions direction="vertical" :column="1" size="medium" border>
        <el-descriptions-item>
          <template slot="label">
            <i class="el-icon-user"></i>
            执行人
          </template>
          {{ recordinfo.salesManId }}
        </el-descriptions-item>
        <el-descriptions-item>
          <template slot="label">
            <i class="el-icon-suitcase"></i>
            记录时间
          </template>
          {{ recordinfo.recordDate | initDate }}
        </el-descriptions-item>
        <el-descriptions-item>
          <template slot="label">
            <i class="el-icon-office-building"></i>
            内容
          </template>
          {{ recordinfo.recordInfo ? recordinfo.recordInfo : '无' }}
        </el-descriptions-item>
        <el-descriptions-item>
          <template slot="label">
            <i class="el-icon-office-building"></i>
            记录方式
          </template>
          {{ recordinfo.recordInfo === 0 ? '手抄本' : recordinfo.recordInfo === 1 ? '日志' : '报表' }}
        </el-descriptions-item>
      </el-descriptions>
    </el-dialog>
    <!-- 修改行动信息弹窗 -->
    <el-dialog title="修改信息" :visible.sync="editDialogVisible" width="50%" @close="editDialogClosed">
      <el-form :model="editRecordForm" :rules="noteFormRules" ref="editFormRef" label-width="80px">
        <!-- 销售员 id -->
        <el-form-item label="销售员 id" prop="salesManId">
          <el-input v-model="editRecordForm.salesManId"></el-input>
        </el-form-item>
        <!-- 客户 id -->
        <el-form-item label="客户 id" prop="customerId">
          <el-input v-model="editRecordForm.customerId"></el-input>
        </el-form-item>
        <el-form-item label="记录时间" prop="recordDate">
          <el-date-picker v-model="editRecordForm.recordDate" value-format="yyyy-MM-dd" type="date" placeholder="选择日期"> </el-date-picker>
        </el-form-item>
        <!-- 计划内容 -->
        <el-form-item label="计划内容" prop="recordInfo">
          <el-input type="textarea" :rows="4" placeholder="请输入合同内容" v-model="editRecordForm.recordInfo"> </el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editRecordInfo">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 添加计划信息弹窗 -->
    <el-dialog title="添加计划" :visible.sync="addDialogVisible" width="50%" center @close="addDialogClosed">
      <!-- 主体内容区域 -->
      <el-form :model="addRecordForm" :rules="noteFormRules" ref="addFormRef" label-width="90px">
        <!-- 销售员 id -->
        <el-form-item label="销售员 id" prop="salesManId">
          <el-input v-model="addRecordForm.salesManId"></el-input>
        </el-form-item>
        <!-- 顾客 id -->
        <el-form-item label="顾客 id" prop="customerId">
          <el-input v-model="addRecordForm.customerId"></el-input>
        </el-form-item>
        <!-- 计划内容 -->
        <el-form-item label="行动内容" prop="recordInfo">
          <el-input type="textarea" :rows="4" placeholder="请输入合同内容" v-model="addRecordForm.recordInfo"> </el-input>
        </el-form-item>
        <!-- 计划方式 -->
        <el-form-item label="记录方式" prop="recordWay">
          <el-radio-group v-model="addRecordForm.recordWay">
            <el-radio :label="0">手抄本</el-radio>
            <el-radio :label="1">日志</el-radio>
            <el-radio :label="2">报表</el-radio>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addRecordInfo">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import BreadCrumb from '@/components/subcomponents/BreadCrumb.vue'
import { initDate } from '@/filters'
import request from '@/utils/request.js'

export default {
  name: 'MyActionNotes',
  components: {
    BreadCrumb
  },
  filters: {
    initDate
  },
  data() {
    return {
      // 行动记录列表
      recordlist: [],
      // 分页效果
      currentPage: 1,
      pagesize: 6,
      // 查找值
      querylist: {
        salesManId: ''
      },
      // 列表总数
      total: 0,
      // 行动记录信息
      recordinfo: [],
      // 查看信息 Dialog 显示值
      infoDialogVisible: false,
      addDialogVisible: false,
      // 编辑信息 Dialog 显示值
      editDialogVisible: false,
      addRecordForm: {
        salesManId: '',
        customerId: '',
        recordDate: '',
        recordInfo: '',
        recordWay: 0
      },
      // 修改行动记录信息
      editRecordForm: {},
      noteFormRules: {
        customerId: [{ required: true, message: '请输入客户 id', trigger: 'blur' }],
        salesManId: [{ required: true, message: '请输入销售员 id', trigger: 'blur' }],
        recordDate: [{ required: true, message: '请选择记录时间', trigger: 'blur' }],
        recordInfo: [{ required: true, message: '请输入记录内容', trigger: 'blur' }]
      }
    }
  },
  created() {
    this.getActionList()
  },
  methods: {
    // 调用服务器接口，获取数据信息
    async getActionList() {
      const { data: res } = await request.post('ActionRecords/find/' + this.pagesize + '/' + this.currentPage, { recordId: '' })
      // 判断获取是否成功
      if (res.status !== 200) return this.$message.error('获取行动记录信息失败!')
      // 将数据保存到 recordlist 中
      this.recordlist = res.data
      this.total = res.total
    },
    // 监听清空搜索输入框事件
    clearRecord() {
      this.getActionList()
    },
    // 监听查询按钮点击事件
    async findRecordById() {
      const { data: res } = await request.get('ActionRecords/Sid/' + this.querylist.salesManId)
      // 判断接口获取是否成功
      if (res.status !== 200) return this.$message.error('获取计划信息失败!')     
      // 将数据保存到 actionlist 中
      this.recordlist = res.data
      this.total = res.total
    },
    // 监听添加按钮点击事件
    addRecordInfo() {
      this.$refs.addFormRef.validate(async valid => {
        if (!valid) return
        // this.addRecordForm.recordDate = new Date().toDateString()
        // 调用接口
        const { data: res } = await request.post('ActionRecords/', this.addRecordForm)
        // 判断添加是否成功
        if (res.status !== 200) {
          this.addDialogVisible = false
          return this.$message.error('添加行动记录失败!')
        }
        // 关闭 addDialog 窗口
        this.addDialogVisible = false
        // 重新获取用户信息数据
        this.getActionList()
        // 显示提示信息
        this.$message.success('添加行动记录成功!')
      })
    },
    // 监听添加 Dialog 关闭事件
    addDialogClosed() {
      this.$refs.addFormRef.resetFields()
    },
    // 监听查看信息按钮点击事件
    async recordInfoDialogShow(id) {
      this.infoDialogVisible = true
      const { data: res } = await request.get('ActionRecords/' + id)
      if (res.status !== 200) return this.$message.error('获取行动记录信息失败!')
      this.recordinfo = res.data
    },
    // 监听编辑信息按钮点击事件
    async editRecordDialogShow(id) {
      const { data: res } = await request.get('ActionRecords/' + id)
      if (res.status !== 200) return this.$message.error('获取行动记录信息失败!')
      this.editRecordForm = res.data
      this.editDialogVisible = true
    },
    // 重置修改用户表单内容
    editDialogClosed() {
      this.$refs.editFormRef.resetFields()
    },
    editRecordInfo() {
      this.$refs.editFormRef.validate(async valid => {
        if (!valid) return
        // 调用接口
        const { data: res } = await request.put('ActionRecords/' + this.editRecordForm.recordId, this.editRecordForm)
        // 判断添加是否成功
        if (res.status !== 200) return this.$message.error('修改计划信息失败!')
        // 关闭 addDialog 窗口
        this.editDialogVisible = false
        // 重新获取投诉建议信息数据
        this.getActionList()
        // 显示提示信息
        this.$message.success('修改计划信息成功!')
      })
    },
    // 监听删除行动记录按钮点击事件
    async deleteRecordInfo(id) {
      const confirmText = await this.$confirm('此操作将永久删除该行动记录, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消删除')
      // 调用删除行动记录信息接口
      const { data: res } = await request.delete('ActionRecords/' + id)
      // 判断删除是否成功，提示相关消息
      if (res.status !== 200) return this.$message.error('删除行动记录失败!')
      this.$message.success('删除行动记录成功!')
      // 重新加载用户数据
      this.getActionList()
    },
    // 改变每页显示条数
    handleSizeChange(pagesize) {
      this.pagesize = pagesize
      this.currentPage = 1
      this.getActionList()
    },
    // 改变显示页
    handleCurrentChange(val) {
      this.currentPage = val
      this.getActionList()
    }
  }
}
</script>

<style lang="less" scoped>
</style>
