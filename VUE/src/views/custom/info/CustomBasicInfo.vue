<template>
  <!-- 客户基础信息管理组件 -->
  <div>
    <!-- 面包屑区域 -->
    <BreadCrumb firstname="客户" secondname="客户基础信息"></BreadCrumb>
    <!-- 搜索区域 -->
    <el-card style="margin-bottom: 15px">
      <el-row :gutter="20">
        <!-- 客户姓名输入框 -->
        <el-col :span="4">
          <el-input v-model="querylist.customerName" placeholder="客户姓名" clearable @clear="clearName"></el-input>
        </el-col>
        <!-- 客户住址输入框 -->
        <el-col :span="6">
          <el-input v-model="querylist.customerAddress" placeholder="客户住址" clearable @clear="clearAddress"></el-input>
        </el-col>
        <!-- 查询按钮 -->
        <el-col :span="9">
          <el-button @click="findUserByName" type="primary">查询</el-button>
        </el-col>
        <!-- 批量删除按钮 -->
        <el-col :span="2">
          <el-button type="primary" @click="deleteContractsInfo">批量删除</el-button>
        </el-col>
        <el-col :span="1">
          <el-button type="primary" @click="addDialogVisible = true">添加用户</el-button>
        </el-col>
      </el-row>
    </el-card>
    <!-- 客户信息区域 -->
    <el-card>
      <!-- 客户信息列表 -->
      <el-table :data="userlist" @selection-change="handleSelectionChange">
        <el-table-column type="selection"> </el-table-column>
        <!-- 日期 -->
        <el-table-column prop="customerId" label="#" sortable> </el-table-column>
        <!-- 姓名 -->
        <el-table-column prop="customerName" label="姓名" sortable> </el-table-column>
        <!-- 地址 -->
        <el-table-column prop="customerAddress" label="地址"> </el-table-column>
        <!-- 联系方式 -->
        <el-table-column prop="customerOccupation" label="行业"> </el-table-column>
        <!-- 操作 -->
        <el-table-column fixed="right" label="操作" width="180">
          <template slot-scope="scope">
            <!-- 查看信息 -->
            <el-button @click="infoDialogShow(scope.row.customerId)" type="primary" icon="el-icon-view" size="small"></el-button>
            <!-- 编辑信息 -->
            <el-button @click="editDialogShow(scope.row.customerId)" type="warning" icon="el-icon-edit" size="small"></el-button>
            <!-- 删除信息 -->
            <el-button @click="deleteUserInfo(scope.row.customerId)" type="danger" icon="el-icon-delete" size="small"></el-button>
          </template>
        </el-table-column>
      </el-table>
      <!-- 分页区域 -->
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page.sync="currentPage"
        :page-size="pagesize"
        :page-sizes="[6, 10, 15]"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </el-card>
    <!-- 查看用户信息弹窗 -->
    <el-dialog title="用户信息" :visible.sync="infoDialogVisible" width="50%">
      <!-- 基础信息区域 -->
      <el-descriptions title="基础信息" :column="3" size="medium" border> </el-descriptions>
      <el-row :gutter="20">
        <el-col :span="6">
          <el-image :src="userinfo.customerPhoto"></el-image>
        </el-col>
        <el-col :span="18">
          <el-descriptions :column="1" direction="vertical" size="medium" border>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-user"></i>
                用户名
              </template>
              {{ userinfo.customerName }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-suitcase"></i>
                行业
              </template>
              {{ userinfo.customerOccupation === null ? '-' : userinfo.customerOccupation }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-office-building"></i>
                联系地址
              </template>
              {{ userinfo.customerAddress }}
            </el-descriptions-item>
          </el-descriptions>
        </el-col>
      </el-row>
      <!-- 扩展信息区域 -->
      <div class="extendinfodiv">
        <el-row>
          <el-col :span="24"><span>扩展信息</span></el-col>
        </el-row>
        <div class="extendinfo">
          <div :v-if="haveinfo">
            <div :key="item.extendId" v-for="item in extendinfo.textlist">
              <el-input
                class="input-new-tag"
                v-if="editable[item.extendId]"
                v-model="item.extendInfo"
                :ref="'editableInput' + item.extendId"
                size="small"
                placeholder="请输入扩展信息"
                @keyup.enter.native="handleEditableInputConfirm(item)"
                @change="handleEditableInputConfirm(item)"
                @blur="handleEditableInputBlur(item)"
                maxlength="20"
                show-word-limit
              ></el-input>
              <el-tag v-else @click="showEditTagInput(item.extendId)" closable :disable-transitions="true" @close="deleteExtendInfo(item.extendId)">{{ item.extendInfo }}</el-tag>
            </div>
          </div>
          <div>
            <el-input
              class="input-new-tag"
              v-if="tagInputVisible"
              v-model="tagInputValue"
              ref="saveTagInput"
              size="small"
              @keyup.enter.native="addExtendInfo"
              @blur="addExtendInfo"
              maxlength="20"
              show-word-limit
            >
            </el-input>
            <el-button v-else class="button-new-tag" size="small" @click="showTagInput">+ 扩展信息</el-button>
          </div>
        </div>
      </div>
    </el-dialog>
    <!-- 添加用户信息弹窗 -->
    <el-dialog title="添加用户" :visible.sync="addDialogVisible" width="50%" center @close="addDialogClosed">
      <!-- 主体内容区域 -->
      <el-form :model="addUserForm" :rules="addFormRules" ref="addFormRef" label-width="80px">
        <!-- 用户名 -->
        <el-form-item label="用户姓名" prop="customerName">
          <el-input v-model="addUserForm.customerName"></el-input>
        </el-form-item>
        <!-- 地址 -->
        <el-form-item label="地址" prop="customerAddress">
          <el-input v-model="addUserForm.customerAddress"></el-input>
        </el-form-item>
        <!-- 行业 -->
        <el-form-item label="行业" prop="customerOccupation">
          <el-input v-model="addUserForm.customerOccupation"></el-input>
        </el-form-item>
        <!-- 头像 -->
        <el-alert title="图片应为 JPG 格式，图片大小不能超过 2MB！" type="warning" :closable="false" show-icon> </el-alert>
        <el-form-item label="头像">
          <el-upload class="avatar-uploader" action="http://81.71.19.235:9659/api/ImageUpload" :show-file-list="false" :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload">
            <img v-if="imageUrl" :src="imageUrl" class="avatar" />
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
        </el-form-item>
      </el-form>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addUserInfo">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 修改用户信息弹窗 -->
    <el-dialog title="修改信息" :visible.sync="editDialogVisible" width="50%" @close="editDialogClosed">
      <el-form :model="editUserForm" :rules="editFormRules" ref="editFormRef" label-width="80px">
        <!-- 用户名 -->
        <el-form-item label="姓名" prop="customerName">
          <el-input v-model="editUserForm.customerName"></el-input>
        </el-form-item>
        <el-form-item label="地址" prop="customerAddress">
          <el-input v-model="editUserForm.customerAddress"></el-input>
        </el-form-item>
        <el-form-item label="行业" prop="customerOccupation">
          <el-input v-model="editUserForm.customerOccupation"></el-input>
        </el-form-item>
        <el-form-item label="头像">
          <el-upload class="avatar-uploader" action="http://81.71.19.235:9659/api/ImageUpload" :show-file-list="false" :on-success="editAvatarSuccess" :before-upload="beforeAvatarUpload">
            <img v-if="editUserForm.customerPhoto" :src="editUserForm.customerPhoto" class="avatar" />
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
          <el-alert title="图片应为 JPG 格式，图片大小不能超过 2MB！" type="warning" :closable="false" show-icon> </el-alert>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editUserInfo">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import BreadCrumb from '@/components/subcomponents/BreadCrumb.vue'
import request from '@/utils/request.js'
import { Loading } from 'element-ui'

export default {
  name: 'MyCustomBasicInfo',
  components: {
    BreadCrumb
  },
  data() {
    // 验证电话规则
    var checkMobile = (rule, value, cb) => {
      // 电话正则表达式
      const regMobile = /^(0| 86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/

      if (regMobile.test(value)) {
        return cb()
      }
      cb(new Error('请输入正确的电话'))
    }

    return {
      // 获取用户列表对象
      userlist: [],
      currentPage: 1,
      pagesize: 6,
      // 获取数据总数
      total: 0,
      querylist: {
        customerName: '',
        customerAddress: ''
      },
      // 添加用户 Dialog 显示参数
      addDialogVisible: false,
      // 添加用户数据对象
      addUserForm: {
        SalesManId: '',
        customerName: '',
        customerOccupation: '',
        customerAddress: '',
        customerPhoto: '',
        CustomerPwd: ''
      },
      // 添加用户规则
      addFormRules: {
        customerName: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
        customerAddress: [{ required: true, message: '请输入用户地址', trigger: 'blur' }],
        mobile: [
          { required: true, message: '请输入电话', trigger: 'blur' },
          { validator: checkMobile, trigge: 'blur' }
        ],
        customerOccupation: [{ required: true, message: '请输入行业', trigger: 'blur' }]
      },
      // 用户信息 Dialog 显示参数
      infoDialogVisible: false,
      // 具体用户信息对象
      userinfo: [],
      // 用户扩展信息对象
      extendinfo: {
        num: 0,
        textlist: []
      },
      haveinfo: true,
      // 标签转输入框参数
      tagInputVisible: false,
      // 获取添加扩展信息的值
      tagInputValue: '',
      editable: [],
      // 修改信息 Dialog 显示参数
      editDialogVisible: false,
      // 修改用户数据对象
      editUserForm: {},
      // 修改用户规则
      editFormRules: {
        customerName: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
        customerAddress: [{ required: true, message: '请输入用户地址', trigger: 'blur' }],
        customerOccupation: [{ required: true, message: '请输入行业', trigger: 'blur' }]
      },
      imageUrl: '',
      imageStoreUrl: 'http://81.71.19.235:9659/api/Images/',
      // 多选数组
      selectedData: []
    }
  },
  created() {
    this.getUserList()
  },
  methods: {
    // 监听批量删除按钮点击事件
    async deleteContractsInfo() {
      if (this.selectedData.length <= 0) return this.$message.info('请先选择数据')
      const confirmText = await this.$confirm('此操作将永久删除这些客户信息, 是否继续?', '提示', {
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
        // 调用删除客户信息接口
        const { data: res } = await request.delete('Customers/' + this.selectedData[index].customerId)
        if (res.status !== 200) {
          loadingInstance.close()
          return this.$message.error(`删除第${++index}个客户信息失败!`)
        }
      }
      loadingInstance.close()
      this.$message.success('删除客户信息成功!')
      this.querylist.customerName = ''
      this.querylist.customerAddress = ''
      this.currentpage = 1
      // 重新加载联系人数据
      this.getUserList()
    },
    // 监听批量选择事件
    handleSelectionChange(val) {
      // 将选中的数据保存到 selectedData 中
      this.selectedData = val
    },
    // 修改客户信息头像图片上传成功后
    editAvatarSuccess(res, file) {
      this.editUserForm.customerPhoto = this.imageStoreUrl + file.response.data.filename
    },
    // 添加客户信息头像图片上传成功后
    handleAvatarSuccess(res, file) {
      this.imageUrl = URL.createObjectURL(file.raw)
      this.addUserForm.customerPhoto = this.imageStoreUrl + file.response.data.filename
    },
    // 添加图片前验证图片是否符合条件 JPG格式、小于 2MB
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
    // 从服务器获取客户数据
    async getUserList() {
      // 调用请求用户数据接口
      const { data: res } = await request.post('Customers/find/' + this.pagesize + '/' + this.currentPage, this.querylist)
      // 判断获取是否成功，若成功，将数据保存到 userlist 中；反之，提示调用失败！
      if (res.status === 500) {
        this.userlist = []
        return this.$message.error('没有用户信息!')
      }
      if (res.status !== 200) return this.$message.error('获取用户信息失败!')
      this.userlist = res.data
      this.total = res.total
    },
    // 改变每页显示条数
    handleSizeChange(newpagesize) {
      this.pagesize = newpagesize
      this.currentPage = 1
      this.getUserList()
    },
    // 改变显示页
    handleCurrentChange(newval) {
      this.currentPage = newval
      this.getUserList()
    },
    // 通过搜索框查询客户
    findUserByName() {
      this.getUserList()
    },
    // 清空姓名搜索框
    clearName() {
      // 重新获取用户信息数据
      this.getUserList()
    },
    // 清空地址搜索框
    clearAddress() {
      // 重新获取用户信息列表
      this.getUserList()
    },
    // 重置添加用户表单内容
    addDialogClosed() {
      this.$refs.addFormRef.resetFields()
      this.addUserForm.customerOccupation = ''
    },
    // 添加用户信息
    addUserInfo() {
      this.$refs.addFormRef.validate(async valid => {
        if (!valid) return
        // 调用添加用户接口
        const { data: res } = await request.post('Customers', this.addUserForm)
        // 判断添加是否成功
        if (res.status !== 200) {
          this.addDialogVisible = false
          return this.$message.error('添加用户信息失败!')
        }
        // 关闭 addDialog 窗口
        this.addDialogVisible = false
        // 重新获取用户信息数据
        this.getUserList()
        // 显示提示信息
        this.$message.success('添加用户信息成功!')
      })
    },
    // 显示用户信息对话框
    async infoDialogShow(id) {
      this.haveinfo = true
      this.infoDialogVisible = true
      // 调用获取用户信息接口
      const { data: res } = await request.get('Customers/' + id)
      // 判断获取是否成功
      if (res.status !== 200) {
        return this.$message.error('获取用户信息失败!')
      }
      this.userinfo = res.data[0]
      // 调用获取用户扩展信息接口
      const { data: resextend } = await request.get('CusExtensioninfoes/cid/' + id)
      // 判断获取是否成功
      if (resextend.total === 0) {
        this.extendinfo.textlist = ''
        this.haveinfo = false
        return
      }
      this.extendinfo.textlist = resextend.data
      this.extendinfo.num = this.extendinfo.textlist.length
    },
    // 显示修改用户信息对话框
    async editDialogShow(id) {
      // 调用获取用户信息接口
      const { data: res } = await request.get('Customers/' + id)
      // 判断获取是否成功
      if (res.status !== 200) {
        return this.$message.error('获取用户信息失败!')
      }
      this.editUserForm = res.data[0]
      this.editDialogVisible = true
    },
    // 重置修改用户表单内容
    editDialogClosed() {
      this.$refs.editFormRef.resetFields()
    },
    // 修改用户信息
    editUserInfo() {
      this.$refs.editFormRef.validate(async valid => {
        if (!valid) return
        // 调用修改用户接口
        const { data: res } = await request.put('Customers/' + this.editUserForm.customerId, this.editUserForm)
        // 判断是否修改成功
        if (res.status !== 200) {
          return this.$message.error('获取用户信息失败!')
        }
        // 若成功，关闭 Dialog 对话框
        this.editDialogVisible = false
        // 重新加载用户数据
        this.getUserList()
        // 提示修改成功
        this.$message.success('修改用户信息成功!')
      })
    },
    // 删除用户信息
    async deleteUserInfo(id) {
      const confirmResult = await this.$confirm('此操作将永久删除该用户信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmResult !== 'confirm') return this.$message.info('已取消删除!')
      // 调用删除用户信息接口
      const { data: res } = await request.delete('Customers/' + id)
      // 判断删除是否成功
      if (res.status !== 200) return this.$message.error('删除用户信息失败!')
      this.querylist.customerName = ''
      this.querylist.customerAddress = ''
      this.currentPage = 1
      // 重新加载用户数据
      this.getUserList()
      // 提示相关消息
      this.$message.success('删除用户信息成功!')
    },
    // 监听删除标签事件
    async deleteExtendInfo(id) {
      // 调用删除扩展信息接口
      const { data: res } = await request.delete('CusExtensioninfoes/' + id)
      // 判断删除是否成功
      if (res.status !== 200) return this.$message.error('删除用户扩展信息失败!')
      // 重新获取数据
      this.infoDialogShow(this.userinfo.customerId)
    },
    // 监听添加标签事件
    async addExtendInfo() {
      const newTagInputValue = this.tagInputValue
      if (newTagInputValue) {
        var tagInfo = {
          customerId: this.userinfo.customerId,
          extendInfo: newTagInputValue
        }
        // 调用插入扩展信息接口
        const { data: res } = await request.post('CusExtensioninfoes/', tagInfo)
        // 判断是否插入成功
        if (res.status !== 200) return this.$message.error('添加用户扩展信息失败!')
        // 将新扩展信息输出到屏幕上
        this.infoDialogShow(this.userinfo.customerId)
      }
      this.tagInputVisible = false
      this.tagInputValue = ''
    },
    // 展示扩展信息输入框按钮
    showTagInput() {
      this.tagInputVisible = true
      this.$nextTick(_ => {
        this.$refs.saveTagInput.$refs.input.focus()
      })
    },
    // 展示扩展信息编辑输入框按钮
    showEditTagInput(id) {
      this.$set(this.editable, id, true)
      this.$nextTick(_ => {
        var editableInput = 'editableInput' + id
        this.$refs[editableInput][0].$refs.input.focus()
      })
    },
    // 判断编辑输入框的信息是否发生改变
    async handleEditableInputConfirm(item) {
      if (item.extendInfo) {
        // 定义对象传给服务器
        var params = {
          customerId: item.customerId,
          extendId: item.extendId,
          extendInfo: item.extendInfo
        }
        // 调用修改扩展信息接口
        const { data: res } = await request.put('CusExtensioninfoes/' + item.extendId, params)
        // 判断修改是否成功
        if (res.status !== 200) return this.$message.error('修改用户扩展信息失败!')
        // 将数据显示在屏幕上
        this.infoDialogShow(this.userinfo.customerId)
      } else {
        this.$message.info('请输入扩展信息')
      }
      this.$set(this.editable, item.extendId, false)
    },
    // 监听编辑扩展信息失去焦点
    async handleEditableInputBlur(item) {
      if (item.extendInfo) {
        // 定义对象传给服务器
        var params = {
          customerId: item.customerId,
          extendId: item.extendId,
          extendInfo: item.extendInfo
        }
        // 调用修改扩展信息接口
        const { data: res } = await request.put('CusExtensioninfoes/' + item.extendId, params)
        // 判断修改是否成功
        if (res.status !== 200) return this.$message.error('修改用户扩展信息失败!')
        // 将数据显示在屏幕上
        this.infoDialogShow(this.userinfo.customerId)
      } else {
        this.$message.info('请输入扩展信息')
      }
      this.$set(this.editable, item.extendId, false)
    }
  }
}
</script>

<style lang="less" scoped>
.el-table .warning-row {
  background: oldlace;
}

.el-table .success-row {
  background: #f0f9eb;
}

.pagidiv {
  margin-top: 10px;
  text-align: center;
}

.el-image {
  width: 178.5px;
  height: 250px;
}

/deep/ .el-alert--warning.is-light {
  width: 50%;
  margin-bottom: 5px;
  margin-left: 75px;
}

.extendinfodiv {
  height: 100%;
  width: 100%;
  .el-row {
    font-size: 16px;
    font-weight: 700;
    color: #303133;
    margin-bottom: 15px;
  }
  .extendinfo {
    padding: 10px;
    div {
      display: inline;
    }
    .el-tag {
      margin-left: 10px;
      margin-bottom: 10px;
    }
    .button-new-tag {
      margin-left: 10px;
      margin-bottom: 10px;
      height: 32px;
      line-height: 30px;
      padding-top: 0;
      padding-bottom: 0;
    }
    .input-new-tag {
      width: 90px;
      margin-left: 10px;
      margin-bottom: 10px;
      vertical-align: bottom;
    }
  }
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
