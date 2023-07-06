<template>
  <!-- 销售员基础信息组件 -->
  <div>
    <!-- 面包屑区域 -->
    <BreadCrumb firstname="销售员" secondname="销售员基础信息"></BreadCrumb>
    <!-- 搜索区域 -->
    <el-card style="margin-bottom: 15px">
      <el-row :gutter="20">
        <!-- 销售员姓名输入框 -->
        <el-col :span="4">
          <el-input v-model="querylist.salesManName" placeholder="销售员姓名" clearable @clear="clearName"></el-input>
        </el-col>
        <!-- 查询按钮 -->
        <el-col :span="11">
          <el-button @click="findSalesByName" type="primary">查询</el-button>
        </el-col>
        <!-- 重置密码按钮 -->
        <el-col :span="3">
          <el-button type="primary" @click="resetSaleMenPwd">重置密码</el-button>
        </el-col>
        <!-- 批量删除按钮 -->
        <el-col :span="3">
          <el-button type="primary" @click="deleteSaleMenInfo">批量删除</el-button>
        </el-col>
        <el-col :span="1">
          <el-button type="primary" @click="addDialogVisible = true">添加销售员</el-button>
        </el-col>
      </el-row>
    </el-card>
    <!-- 销售员信息区域 -->
    <el-card>
      <!-- 销售员信息列表 -->
      <el-table :data="salesManList" @selection-change="handleSelectionChange">
        <el-table-column type="selection"> </el-table-column>
        <!-- 日期 -->
        <el-table-column prop="salesManId" label="#" sortable> </el-table-column>
        <!-- 姓名 -->
        <el-table-column prop="salesManName" label="姓名" sortable> </el-table-column>
        <!-- 电话 -->
        <el-table-column prop="salesManPhone" label="电话"> </el-table-column>
        <!-- 邮箱 -->
        <el-table-column prop="salesManEmail" label="邮箱"> </el-table-column>
        <!-- 分组 -->
        <el-table-column prop="groupId" label="分组" width="120">
          <!-- 将状态进行渲染，用不同颜色的标签显示不同的状态 -->
          <template slot-scope="scope">
            <el-tag size="small" :type="scope.row.groupId === 1 ? 'primary' : scope.row.groupId === 2 ? 'warning' : scope.row.groupId === 3 ? 'success' : 'info'">
              {{ scope.row.groupName === null ? '默认组' : scope.row.groupName }}
            </el-tag>
          </template>
        </el-table-column>
        <!-- 操作 -->
        <el-table-column fixed="right" label="操作" width="180">
          <template slot-scope="scope">
            <!-- 查看信息 -->
            <el-button @click="infoDialogShow(scope.row.salesManId)" type="primary" icon="el-icon-view" size="small"></el-button>
            <!-- 编辑信息 -->
            <el-button @click="editDialogShow(scope.row.salesManId)" type="warning" icon="el-icon-edit" size="small"></el-button>
            <!-- 删除信息 -->
            <el-button @click="deleteSalesManInfo(scope.row.salesManId)" type="danger" icon="el-icon-delete" size="small"></el-button>
          </template>
        </el-table-column>
      </el-table>
      <!-- 分页区域 -->
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page.sync="currentPage"
        :page-size="pageSize"
        :page-sizes="[6, 10, 15]"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </el-card>
    <!-- 添加联系人信息弹窗 -->
    <el-dialog title="添加联系人" :visible.sync="addDialogVisible" width="50%" center @close="addDialogClosed">
      <!-- 主体内容区域 -->
      <el-form :model="addSalesManForm" :rules="salesmenFormRules" ref="addFormRef" label-width="70px">
        <!-- 姓名 -->
        <el-form-item label="姓名" prop="salesManName">
          <el-input v-model="addSalesManForm.salesManName"></el-input>
        </el-form-item>
        <!-- 密码 -->
        <el-alert title="密码可填可不填，默认为 '123456'" type="success" :closable="false" center show-icon> </el-alert>
        <el-form-item label="密码" prop="salesManPwd">
          <el-input type="password" show-password v-model="addSalesManForm.salesManPwd"></el-input>
        </el-form-item>
        <!-- 确认密码 -->
        <el-alert title="若密码没填，这项也不填" type="success" :closable="false" center show-icon> </el-alert>
        <el-form-item label="确认密码" prop="confirmPwd">
          <el-input type="password" show-password v-model="confirmPwd"></el-input>
        </el-form-item>
        <!-- 电话 -->
        <el-form-item label="电话" prop="salesManPhone">
          <el-input v-model="addSalesManForm.salesManPhone"></el-input>
        </el-form-item>
        <!-- 邮箱 -->
        <el-form-item label="邮箱" prop="salesManEmail">
          <el-input v-model="addSalesManForm.salesManEmail"></el-input>
        </el-form-item>
        <!-- 头像 -->
        <el-alert title="图片应为 JPG 格式，图片大小不能超过 2MB！" type="warning" :closable="false" center show-icon> </el-alert>
        <el-form-item label="头像">
          <el-upload class="avatar-uploader" action="http://81.71.19.235:9659/api/ImageUpload" :show-file-list="false" :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload">
            <img v-if="imgUrl" :src="imgUrl" class="avatar" />
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
        </el-form-item>
      </el-form>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addSalesManInfo">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 查看销售员信息弹窗 -->
    <el-dialog title="销售员信息" :visible.sync="infoDialogVisible" width="50%">
      <el-row :gutter="20">
        <el-col :span="6">
          <el-image :src="salesManInfo.salesManImg"></el-image>
        </el-col>
        <el-col :span="18">
          <el-descriptions :column="2" size="medium" border>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-collection-tag"></i>
                Id
              </template>
              {{ salesManInfo.salesManId }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-user"></i>
                姓名
              </template>
              {{ salesManInfo.salesManName }}
            </el-descriptions-item>
          </el-descriptions>
          <el-descriptions :column="1" direction="vertical" size="medium" border>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-phone-outline"></i>
                电话
              </template>
              {{ salesManInfo.salesManPhone }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-message"></i>
                邮箱
              </template>
              {{ salesManInfo.salesManEmail }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-more-outline"></i>
                分组
              </template>
              <el-tag size="small" type="success">
                {{ salesManInfo.groupName === null ? '默认组' : salesManInfo.groupName }}
              </el-tag>
            </el-descriptions-item>
          </el-descriptions>
        </el-col>
      </el-row>
    </el-dialog>
    <!-- 修改联系人信息弹窗 -->
    <el-dialog title="修改信息" :visible.sync="editDialogVisible" width="50%" @close="editDialogClosed">
      <el-form :model="editSalesManForm" :rules="salesmenFormRules" ref="editFormRef" label-width="70px">
        <!-- 用户名 -->
        <el-form-item label="姓名" prop="salesManName">
          <el-input v-model="editSalesManForm.salesManName"></el-input>
        </el-form-item>
        <el-form-item label="电话" prop="salesManPhone">
          <el-input v-model="editSalesManForm.salesManPhone"></el-input>
        </el-form-item>
        <el-form-item label="邮箱" prop="salesManEmail">
          <el-input v-model="editSalesManForm.salesManEmail"></el-input>
        </el-form-item>
        <el-alert title="图片应为 JPG 格式，图片大小不能超过 2MB！" type="warning" :closable="false" show-icon> </el-alert>
        <el-form-item label="头像">
          <el-upload class="avatar-uploader" action="http://81.71.19.235:9659/api/ImageUpload" :show-file-list="false" :on-success="editAvatarSuccess" :before-upload="beforeAvatarUpload">
            <img v-if="editSalesManForm.salesManImg" :src="editSalesManForm.salesManImg" class="avatar" />
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editSalesManInfo">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import BreadCrumb from '@/components/subcomponents/BreadCrumb.vue'
import request from '@/utils/request.js'
import { Loading } from 'element-ui'

export default {
  name: 'MySalesBasicInfo',
  components: {
    BreadCrumb
  },
  data() {
    // 验证密码规则
    var checkPassword = (rule, val, callback) => {
      if (this.confirmPwd === this.addSalesManForm.salesManPwd) return callback()
      callback(new Error('两次密码不一致'))
    }
    // 验证电话规则
    var checkMobile = (rule, val, callback) => {
      // 电话正则表达式
      const regMobile = /^(0| 86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/

      if (regMobile.test(val)) {
        return callback()
      }
      callback(new Error('请输入正确的电话'))
    }
    // 验证邮箱规则
    var checkEmail = (rule, val, callback) => {
      // 邮箱正则表达式
      const regEmail = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,})$/
      if (regEmail.test(val)) {
        return callback()
      }
      callback(new Error('请输入正确的邮箱'))
    }
    return {
      // 销售员
      salesManList: [],
      // 查询的页码值、数据条数、名字
      currentPage: 1,
      pageSize: 6,
      querylist: {
        salesManName: ''
      },
      // 数据总数
      total: 0,
      // 添加信息 Dialog 显示参数
      addDialogVisible: false,
      // 添加销售员信息对象
      addSalesManForm: {
        salesManName: '',
        salesManPwd: '',
        salesManPhone: '',
        salesManEmail: '',
        salesManImg: '',
        groupId: 4,
        groupName: '默认'
      },
      confirmPwd: '',
      imgUrl: '',
      imgStoreUrl: 'http://81.71.19.235:9659/api/Images/',
      // 验证规则
      salesmenFormRules: {
        salesManName: [{ required: true, message: '请输入销售员姓名', trigger: 'blur' }],
        salesManPwd: [{ min: 6, max: 15, message: '长度在 6~15 个字符之间', trigger: 'blur' }],
        confirmPwd: [{ validator: checkPassword, trigge: 'blur' }],
        salesManPhone: [
          { required: true, message: '请输入销售员电话', trigger: 'blur' },
          { validator: checkMobile, trigge: 'blur' }
        ],
        salesManEmail: [
          { required: true, message: '请输入销售员邮箱', trigger: 'blur' },
          { validator: checkEmail, trigge: 'blur' }
        ]
      },
      // 多选数组
      selectedData: [],
      // 查看信息 Dialog 显示参数
      infoDialogVisible: false,
      // 查看销售员信息对象
      salesManInfo: {},
      // 修改信息 Dialog 显示参数
      editDialogVisible: false,
      // 修改销售员信息对象
      editSalesManForm: {}
    }
  },
  created() {
    this.getSalesMenList()
  },
  methods: {
    // 调用接口获取数据
    async getSalesMenList() {
      const { data: res } = await request.post('Salesmen/find/' + this.pageSize + '/' + this.currentPage, this.querylist)
      // 判断获取是否成功
      if (res.status === 500) return this.$message.info('没有销售员信息')
      if (res.status !== 200) return this.$message.error('获取销售员信息失败')
      this.salesManList = res.data
      this.total = res.total
    },
    // 监听销售员姓名输入框清空事件
    clearName() {
      this.currentPage = 1
      this.getSalesMenList()
    },
    // 监听查询按钮点击事件
    findSalesByName() {
      this.currentPage = 1
      this.getSalesMenList()
    },
    // 监听多选选择事件
    handleSelectionChange(selectedVal) {
      this.selectedData = selectedVal
    },
    // 监听批量删除按钮点击事件
    async deleteSaleMenInfo() {
      if (this.selectedData.length <= 0) return this.$message.info('请先选择数据')
      const confirmText = await this.$confirm('此操作将永久删除这些销售员信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消删除')
      let loadingInstance = Loading.service({
        text: '玩命加载中',
        spinner: 'el-icon-loading'
      })
      for (let index = 0; index < this.selectedData.length; index++) {
        // 调用删除信息接口
        const { data: res } = await request.delete('Salesmen/' + this.selectedData[index].salesManId)
        if (res.status !== 200) {
          loadingInstance.close()
          return this.$message.error(`删除第${++index}个销售员信息失败!`)
        }
      }
      loadingInstance.close()
      this.$message.success('删除销售员信息成功!')
      this.querylist.salesManName = ''
      this.currentpage = 1
      // 重新加载销售员数据
      this.getSalesMenList()
    },
    // 监听重置密码按钮点击事件
    async resetSaleMenPwd() {
      if (this.selectedData.length <= 0) return this.$message.info('请先选择数据')
      const confirmText = await this.$confirm('此操作将重置该销售员密码为 123456, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消删除')
      let loadingInstance = Loading.service({
        text: '玩命加载中',
        spinner: 'el-icon-loading'
      })
      for (let index = 0; index < this.selectedData.length; index++) {
        // 调用修改信息接口
        const { data: res } = await request.post('Salesmen/RePwd/' + this.selectedData[index].salesManId)
        if (res.status !== 200) {
          loadingInstance.close()
          return this.$message.error(`重置第${++index}个销售员密码失败!`)
        }
      }
      loadingInstance.close()
      this.$message.success('重置销售员密码成功!')
      this.querylist.salesManName = ''
      this.currentpage = 1
      // 重新加载销售员数据
      this.getSalesMenList()
    },
    // 监听添加信息 Dialog 关闭事件
    addDialogClosed() {
      this.$refs.addFormRef.resetFields()
      this.addSalesManForm.groupId = 0
    },
    // 销售员图片上传前事件
    beforeAvatarUpload(file) {
      const isJPG = file.type === 'image/jpeg'
      const isLt2M = file.size / 1024 / 1024 < 2

      if (!isJPG) {
        this.$message.error('上传头像图片只能是 JPG 格式!')
      }
      if (!isLt2M) {
        this.$message.error('上传头像图片大小不能超过 2MB!')
      }
      return isJPG && isLt2M
    },
    // 监听销售员图片上传成功后事件
    handleAvatarSuccess(res, file) {
      this.imgUrl = URL.createObjectURL(file.raw)
      this.addSalesManForm.salesManImg = this.imgStoreUrl + file.response.data.filename
    },
    // 监听添加按钮点击事件
    async addSalesManInfo() {
      if (!this.addSalesManForm.salesManPwd) this.addSalesManForm.salesManPwd = '123456'
      const { data: res } = await request.post('Salesmen', this.addSalesManForm)
      if (res.status !== 200) return this.$message.error('添加销售员信息失败')
      this.addSalesManForm.salesManImg = ''
      this.addDialogVisible = false
      this.getSalesMenList()
      this.$message.success('添加销售员信息成功')
    },
    // 监听查看按钮点击事件
    async infoDialogShow(id) {
      const { data: res } = await request.get('Salesmen/id/' + id)
      if (res.status !== 200) return this.$message.error('获取信息失败!')
      this.salesManInfo = res.data[0]
      this.infoDialogVisible = true
    },
    // 监听编辑按钮点击事件
    async editDialogShow(id) {
      const { data: res } = await request.get('Salesmen/id/' + id)
      if (res.status !== 200) return this.$message.error('获取信息失败!')
      this.editSalesManForm = res.data[0]
      this.editDialogVisible = true
    },
    // 监听编辑信息 Dialog 关闭事件
    editDialogClosed() {
      this.$refs.editFormRef.resetFields()
    },
    // 监听修改联系人头像上传图片后事件
    editAvatarSuccess(res, file) {
      this.editSalesManForm.salesManImg = this.imgStoreUrl + file.response.data.filename
    },
    // 监听修改按钮点击事件
    async editSalesManInfo() {
      const { data: res } = await request.put('Salesmen/' + this.editSalesManForm.salesManId, this.editSalesManForm)
      if (res.status !== 200) return this.$message.error('修改销售员信息失败')
      this.editDialogVisible = false
      this.getSalesMenList()
      this.$message.success('修改销售员信息成功')
    },
    // 监听删除按钮点击事件
    async deleteSalesManInfo(id) {
      const confirmResult = await this.$confirm('此操作将永久删除该联系人信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmResult !== 'confirm') return this.$message.info('已取消删除!')
      // 调用删除信息接口
      const { data: res } = await request.delete('Salesmen/' + id)
      // 判断添加是否成功
      if (res.status !== 200) return this.$message.error('删除联系人信息失败!')
      this.querylist.salesManName = ''
      this.currentPage = 1
      // 重新加载联系人数据
      this.getSalesMenList()
      // 提示相关消息
      this.$message.success('删除联系人信息成功!')
    },
    // 监听改变显示信息条数事件
    handleSizeChange(newSize) {
      this.pageSize = newSize
      this.currentPage = 1
      this.getSalesMenList()
    },
    // 监听改变页码值事件
    handleCurrentChange(newVal) {
      this.currentPage = newVal
      this.getSalesMenList()
    }
  }
}
</script>

<style lang="less" scoped>
/deep/ .el-alert--success.is-light {
  width: 40%;
  margin-bottom: 5px;
  margin-left: 70px;
}
/deep/ .el-alert--warning.is-light {
  width: 50%;
  margin-bottom: 5px;
  margin-left: 70px;
}
/deep/ .avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
/deep/ .avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
/deep/ .avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 178.5px;
  height: 250px;
  line-height: 250px;
  text-align: center;
}
/deep/ .avatar {
  width: 178.5px;
  height: 250px;
  display: block;
}
</style>
