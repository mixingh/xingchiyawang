<template>
  <div>
    <!-- 面包屑区域 -->
    <BreadCrumb firstname="客户" secondname="签单人信息"></BreadCrumb>
    <!-- 搜索区域 -->
    <el-card style="margin-bottom: 15px">
      <el-row :gutter="20">
        <!-- 签单人 id 输入框 -->
        <el-col :span="6">
          <el-input v-model="querylist.signerName" placeholder="签单人姓名" clearable @clear="clearContracterName"></el-input>
        </el-col>
        <!-- 查询按钮 -->
        <el-col :span="16">
          <el-button @click="findContractersById" type="primary">查询</el-button>
        </el-col>
        <!-- 添加按钮 -->
        <el-col :span="1">
          <el-button @click="addContracterDialogVisible = true" type="primary">添加</el-button>
        </el-col>
      </el-row>
    </el-card>
    <!-- 签单人信息区域 -->
    <el-card>
      <!-- 签单人信息列表 -->
      <el-table :data="contracterlist">
        <!-- <el-table :data="userlist.slice((currentPage-1)*pagesize,currentPage*pagesize)"> -->
        <!-- 日期 -->
        <el-table-column prop="singerId" label="#" sortable> </el-table-column>
        <!-- 姓名 -->
        <el-table-column prop="signerName" label="姓名" sortable> </el-table-column>
        <!-- 邮箱 -->
        <el-table-column prop="signerEmail" label="邮箱"> </el-table-column>
        <!-- 电话 -->
        <el-table-column prop="signerPhone" label="电话"> </el-table-column>
        <!-- 操作 -->
        <el-table-column fixed="right" label="操作" width="180">
          <template slot-scope="scope">
            <!-- 查看信息 -->
            <el-button @click="contracterInfoDialogShow(scope.row.singerId)" type="primary" icon="el-icon-view" size="small"></el-button>
            <!-- 编辑信息 -->
            <el-button @click="editContracterDialogShow(scope.row.singerId)" type="warning" icon="el-icon-edit" size="small"></el-button>
            <!-- 删除信息 -->
            <el-button @click="deleteContracterInfo(scope.row.singerId)" type="danger" icon="el-icon-delete" size="small"></el-button>
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
    <!-- 添加签单人信息弹窗 -->
    <el-dialog title="添加签单人信息" :visible.sync="addContracterDialogVisible" width="50%" center @close="addContracterDialogClosed">
      <!-- 主体内容区域 -->
      <el-form :model="addContracterForm" :rules="contracterFormRules" ref="addContracterFormRef" label-width="55px">
        <!-- 姓名 -->
        <el-form-item label="姓名" prop="signerName">
          <el-input v-model="addContracterForm.signerName"></el-input>
        </el-form-item>
        <!-- 姓名 -->
        <el-form-item label="邮箱" prop="signerEmail">
          <el-input v-model="addContracterForm.signerEmail"></el-input>
        </el-form-item>
        <!-- 姓名 -->
        <el-form-item label="电话" prop="signerPhone">
          <el-input v-model="addContracterForm.signerPhone"></el-input>
        </el-form-item>
        <!-- 头像 -->
        <el-form-item label="头像">
          <el-row :gutter="10">
            <el-col :span="8">
              <el-upload class="avatar-uploader" action="http://81.71.19.235:9659/api/ImageUpload" :show-file-list="false" :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload">
                <img v-if="imageUrl" :src="imageUrl" class="avatar" />
                <i v-else class="el-icon-plus avatar-uploader-icon"></i>
              </el-upload>
            </el-col>
            <el-col :span="15">
              <el-alert title="图片应为 JPG 格式，图片大小不能超过 2MB！" type="warning" :closable="false" show-icon> </el-alert>
            </el-col>
          </el-row>
        </el-form-item>
        <!-- 签名图片 -->
        <el-form-item label="签名">
          <el-upload class="avatar-uploader" action="http://81.71.19.235:9659/api/ImageUpload" :show-file-list="false" :on-success="handlesignatureImgSuccess" :before-upload="beforeAvatarUpload">
            <img v-if="signatureImgUrl" :src="signatureImgUrl" class="signatureImg" />
            <i v-else class="el-icon-plus avatar-uploader-signature-icon"></i>
          </el-upload>
        </el-form-item>
      </el-form>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="addContracterDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addContracterInfo">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 查看签单人信息弹窗 -->
    <el-dialog title="签单人信息" :visible.sync="infoDialogVisible" width="50%">
      <el-row :gutter="20">
        <el-col :span="6">
          <el-image :src="contracterInfo.signerImg" class="signerImg"></el-image>
        </el-col>
        <el-col :span="18">
          <el-descriptions :column="1" direction="vertical" size="medium" border>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-user"></i>
                姓名
              </template>
              {{ contracterInfo.signerName }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-user"></i>
                电话
              </template>
              {{ contracterInfo.signerPhone }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-suitcase"></i>
                邮箱
              </template>
              {{ contracterInfo.signerEmail }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template slot="label">
                <i class="el-icon-suitcase"></i>
                签名图片
              </template>
              <el-image :src="contracterInfo.signatureImg" class="signatureImg"></el-image>
            </el-descriptions-item>
          </el-descriptions>
        </el-col>
      </el-row>
    </el-dialog>
    <!-- 修改签单人信息弹窗 -->
    <el-dialog title="修改签单人信息" :visible.sync="editContracterDialogVisible" width="50%" @close="editDialogClosed">
      <el-form :model="editContracterForm" :rules="contracterFormRules" ref="editFormRef" label-width="80px">
        <!-- 用户名 -->
        <el-form-item label="姓名" prop="signerName">
          <el-input v-model="editContracterForm.signerName"></el-input>
        </el-form-item>
        <el-form-item label="电话" prop="signerPhone">
          <el-input v-model="editContracterForm.signerPhone"></el-input>
        </el-form-item>
        <el-form-item label="邮箱" prop="signerEmail">
          <el-input v-model="editContracterForm.signerEmail"></el-input>
        </el-form-item>
        <!-- 头像 -->
        <el-form-item label="头像">
          <el-row :gutter="10">
            <el-col :span="8">
              <el-upload class="avatar-uploader" action="http://81.71.19.235:9659/api/ImageUpload" :show-file-list="false" :on-success="editAvatarSuccess" :before-upload="beforeAvatarUpload">
                <img v-if="editContracterForm.signerImg" :src="editContracterForm.signerImg" class="avatar" />
                <i v-else class="el-icon-plus avatar-uploader-icon"></i>
              </el-upload>
            </el-col>
            <el-col :span="15">
              <el-alert title="图片应为 JPG 格式，图片大小不能超过 2MB！" type="warning" :closable="false" show-icon> </el-alert>
            </el-col>
          </el-row>
        </el-form-item>
        <!-- 签名图片 -->
        <el-form-item label="签名">
          <el-upload class="avatar-uploader" action="http://81.71.19.235:9659/api/ImageUpload" :show-file-list="false" :on-success="editSignatureImgSuccess" :before-upload="beforeAvatarUpload">
            <img v-if="editContracterForm.signatureImg" :src="editContracterForm.signatureImg" class="signatureImg" />
            <i v-else class="el-icon-plus avatar-uploader-signature-icon"></i>
          </el-upload>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editContracterDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editContracterInfo">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import BreadCrumb from '@/components/subcomponents/BreadCrumb.vue'
import request from '@/utils/request.js'

export default {
  name: 'MyContracterInfo',
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
    // 验证邮箱规则
    var checkEmail = (rule, value, cb) => {
      // 邮箱正则表达式
      const regEmail = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,})$/

      if (regEmail.test(value)) {
        return cb()
      }
      cb(new Error('请输入正确的邮箱'))
    }
    return {
      contracterlist: [],
      currentPage: 1,
      pagesize: 6,
      total: 0,
      querylist: {
        signerName: ''
      },
      // 编辑 Dialog 显示参数
      editContracterDialogVisible: false,
      addContracterForm: {
        signerName: '',
        signerEmail: '',
        signerPhone: '',
        signatureImg: '',
        signerImg: ''
      },
      addContracterDialogVisible: false,
      contracterFormRules: {
        signerName: [{ required: true, message: '请输入姓名', trigger: 'blur' }],
        signerEmail: [
          { required: true, message: '请输入邮箱', trigger: 'blur' },
          { validator: checkEmail, trigge: 'blur' }
        ],
        signerPhone: [
          { required: true, message: '请输入电话号码', trigger: 'blur' },
          { validator: checkMobile, trigge: 'blur' }
        ]
      },
      editContracterForm: {},
      contracterInfo: {},
      infoDialogVisible: false,
      imageUrl: '',
      signatureImgUrl: '',
      imageStoreUrl: 'http://81.71.19.235:9659/api/Images/'
    }
  },
  created() {
    this.getContracterList()
  },
  methods: {
    // 获取签单人数据
    async getContracterList() {
      // 调用服务器接口获取数据
      const { data: res } = await request.post('Signerinfoes/find/' + this.pagesize + '/' + this.currentPage, this.querylist)
      // 判断获取是否成功
      if (res.status === 500) {
        this.contracterlist = []
        return this.$message.error('没有用户信息!')
      }
      if (res.status !== 200) return this.$message.error('获取签单人信息失败!')
      // 将数据保存到 contracterlist 中
      this.contracterlist = res.data
      this.total = res.total
    },
    // 监听清空签单人 id 输入框清空事件
    clearContracterName() {
      this.getContracterList()
    },
    // 监听搜索签单人按钮事件
    async findContractersById() {
      this.getContracterList()
    },
    // 监听添加按钮点击事件
    addContracterInfo() {
      this.$refs.addContracterFormRef.validate(async valid => {
        if (!valid) return
        // 调用接口
        const { data: res } = await request.post('Signerinfoes', this.addContracterForm)
        // 判断添加是否成功
        if (res.status !== 200) return this.$message.error('添加签单人信息失败!')
        // 关闭 addDialog 窗口
        this.addContracterDialogVisible = false
        this.imageUrl = ''
        this.signatureImgUrl = ''
        // 重新获取信息数据
        this.getContracterList()
        // 显示提示信息
        this.$message.success('添加签单人信息成功!')
      })
    },
    // 监听添加 Dialog 关闭事件
    addContracterDialogClosed() {
      this.imageUrl = ''
      this.signatureImgUrl = ''
      this.$refs.addContracterFormRef.resetFields()
    },
    // 查看信息 Dialog 显示按钮
    async contracterInfoDialogShow(id) {
      const { data: res } = await request.get('Signerinfoes/' + id)
      if (res.status !== 200) return this.$message.error('获取信息失败!')
      this.contracterInfo = res.data
      this.infoDialogVisible = true
    },
    // 编辑信息 Dialog 显示按钮
    async editContracterDialogShow(id) {
      const { data: res } = await request.get('Signerinfoes/' + id)
      // 判断获取是否成功
      if (res.status !== 200) return this.$message.error('获取信息失败!')
      // 将数据保存到 adviceinfo 中
      this.editContracterForm = res.data
      this.editContracterDialogVisible = true
    },
    // 监听修改按钮点击事件
    editContracterInfo() {
      this.$refs.editFormRef.validate(async valid => {
        if (!valid) return
        // 调用接口
        const { data: res } = await request.put('Signerinfoes/' + this.editContracterForm.singerId, this.editContracterForm)
        // 判断修改是否成功
        if (res.status !== 200) return this.$message.error('修改签单人信息失败!')
        // 关闭 addDialog 窗口
        this.editContracterDialogVisible = false
        // 重新获取信息数据
        this.getContracterList()
        // 显示提示信息
        this.$message.success('修改签单人信息成功!')
      })
    },
    // 监听修改 Dialog 关闭事件
    editDialogClosed() {
      this.$refs.editFormRef.resetFields()
    },
    // 监听删除信息按钮事件
    async deleteContracterInfo(id) {
      const confirmResult = await this.$confirm('此操作将永久删除该签单人信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmResult !== 'confirm') return this.$message.info('已取消删除!')
      // 调用删除用户信息接口
      const { data: res } = await request.delete('Signerinfoes/' + id)
      // 判断添加是否成功
      if (res.status !== 200) return this.$message.error('删除签单人信息失败!')
      this.querylist.signerName = ''
      this.currentPage = 1
      // 重新加载用户数据
      this.getContracterList()
      this.$message.success('删除签单人信息成功!')
    },
    // 改变每页显示条数
    handleSizeChange(pagesize) {
      this.pagesize = pagesize
    },
    // 改变显示页
    handleCurrentChange(val) {
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
    // 添加签单人信息头像图片上传成功后
    handleAvatarSuccess(res, file) {
      this.imageUrl = URL.createObjectURL(file.raw)
      this.addContracterForm.signerImg = this.imageStoreUrl + file.response.data.filename
    },
    // 添加签单人签名图片上传成功后
    handlesignatureImgSuccess(res, file) {
      this.signatureImgUrl = URL.createObjectURL(file.raw)
      this.addContracterForm.signatureImg = this.imageStoreUrl + file.response.data.filename
    },
    // 修改签单人信息头像图片上传成功后
    editAvatarSuccess(res, file) {
      this.editContracterForm.signerImg = this.imageStoreUrl + file.response.data.filename
    },
    // 修改签单人签名图片上传成功后
    editSignatureImgSuccess(res, file) {
      this.editContracterForm.signatureImg = this.imageStoreUrl + file.response.data.filename
    }
  }
}
</script>

<style lang="less" scoped>
.signerImg {
  height: 250px;
  width: 180px;
}
.signatureImg {
  height: 150px;
  width: 320px;
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
.avatar-uploader-signature-icon {
  font-size: 28px;
  color: #8c939d;
  width: 320px;
  height: 150px;
  line-height: 150px;
  text-align: center;
}
.signatureImg {
  width: 320px;
  height: 150px;
  display: block;
}
</style>
