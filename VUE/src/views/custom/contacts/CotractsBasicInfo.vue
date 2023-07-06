<template>
  <!-- 联系人基础信息组件 -->
  <div>
    <!-- 面包屑区域 -->
    <BreadCrumb firstname="客户" secondname="联系人基础信息"></BreadCrumb>
    <!-- 搜索区域 -->
    <el-card style="margin-bottom: 15px">
      <el-row :gutter="20">
        <!-- 联系人姓名输入框 -->
        <el-col :span="4">
          <el-input v-model="querylist.contactName" placeholder="联系人姓名" clearable @clear="clearContracterName"></el-input>
        </el-col>
        <!-- 联系人地址输入框 -->
        <el-col :span="6">
          <el-input v-model="querylist.contactAddress" placeholder="联系人地址" clearable @clear="clearContracterAddress"></el-input>
        </el-col>
        <!-- 查询按钮 -->
        <el-col :span="5">
          <el-button @click="findContractByName" type="primary">查询</el-button>
        </el-col>
        <!-- 批量删除按钮 -->
        <el-col :span="3">
          <el-button type="primary" @click="deleteContractsInfo">批量删除</el-button>
        </el-col>
        <!-- 导出联系人信息按钮 -->
        <el-col :span="3">
          <el-button type="primary" @click="outputContacts" :disabled="addButtonDisable">导出联系人</el-button>
        </el-col>
        <!-- 添加联系人按钮 -->
        <el-col :span="3">
          <el-button type="primary" @click="addContractsDialogVisible = true">添加联系人</el-button>
        </el-col>
      </el-row>
    </el-card>
    <!-- 联系人信息区域 -->
    <el-card>
      <!-- 联系人信息列表 -->
      <el-table :data="contractslist" @selection-change="handleSelectionChange">
        <el-table-column type="selection"> </el-table-column>
        <!-- id -->
        <el-table-column prop="contactId" label="#" sortable> </el-table-column>
        <!-- 姓名 -->
        <el-table-column prop="contactName" label="姓名" sortable> </el-table-column>
        <!-- 邮箱 -->
        <el-table-column prop="contactEmail" label="邮箱"> </el-table-column>
        <!-- 联系电话 -->
        <el-table-column prop="contactPhone" label="电话"> </el-table-column>
        <!-- 地址 -->
        <el-table-column prop="contactAddress" label="地址"> </el-table-column>
        <!-- 操作 -->
        <el-table-column fixed="right" label="操作" width="180">
          <template slot-scope="scope">
            <!-- 编辑信息 -->
            <el-button @click="editContractsDialogShow(scope.row.contactId)" type="warning" icon="el-icon-edit" size="small"></el-button>
            <!-- 删除信息 -->
            <el-button @click="deleteContractInfo(scope.row.contactId)" type="danger" icon="el-icon-delete" size="small"></el-button>
          </template>
        </el-table-column>
      </el-table>
      <!-- 分页区域 -->
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="currentpage"
        :page-size="pagesize"
        :page-sizes="[6, 10, 15, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </el-card>
    <!-- 添加联系人信息弹窗 -->
    <el-dialog title="添加联系人" :visible.sync="addContractsDialogVisible" width="75%" center @close="addContractDialogClosed">
      <!-- 头部标签页 -->
      <el-tabs v-model="activeName" @tab-click="handleClick">
        <!-- 单项添加页 -->
        <el-tab-pane label="单项添加" name="first">
          <!-- 主体内容区域 -->
          <el-form :model="addContractForm" :rules="ContractFormRules" ref="addContractFormRef" label-width="80px">
            <!-- 姓名 -->
            <el-form-item label="姓名" prop="contactName">
              <el-input v-model="addContractForm.contactName"></el-input>
            </el-form-item>
            <!-- 邮箱 -->
            <el-form-item label="邮箱" prop="contactEmail">
              <el-input v-model="addContractForm.contactEmail"></el-input>
            </el-form-item>
            <!-- 电话 -->
            <el-form-item label="电话" prop="contactPhone">
              <el-input v-model="addContractForm.contactPhone"></el-input>
            </el-form-item>
            <!-- 地址 -->
            <el-form-item label="地址" prop="contactAddress">
              <el-input v-model="addContractForm.contactAddress"></el-input>
            </el-form-item>
          </el-form>
        </el-tab-pane>
        <!-- 批量添加页 -->
        <el-tab-pane label="批量添加" name="second">
          <!-- 初始显示的添加 Excel 文档按钮 -->
          <div class="buttonBox">
            <el-upload action accept=".xlsx, .xls" :auto-upload="false" :show-file-list="false" :on-change="handle">
              <el-button type="primary" slot="trigger">选择Excel文档</el-button>
            </el-upload>
            <el-alert title="将数据上传至服务器之前，请认真核实您的数据，在确保无误后点击确定按钮即可上传" type="warning" :closable="false" show-icon> </el-alert>
          </div>
          <!-- 数据显示的表格 -->
          <div class="tableBox">
            <el-table :data="tempData" style="width: 100%" border>
              <!-- 姓名 -->
              <el-table-column prop="contactName" label="姓名"> </el-table-column>
              <!-- 邮箱 -->
              <el-table-column prop="contactEmail" label="邮箱"> </el-table-column>
              <!-- 联系电话 -->
              <el-table-column prop="contactPhone" label="电话"> </el-table-column>
              <!-- 地址 -->
              <el-table-column prop="contactAddress" label="地址"> </el-table-column>
            </el-table>
          </div>
        </el-tab-pane>
      </el-tabs>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="addContractsDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addContractInfo" :disabled="submitButtonDisable">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 编辑联系人信息弹窗 -->
    <el-dialog title="修改信息" :visible.sync="editContractDialogVisible" width="50%" @close="editContractDialogClosed">
      <el-form :model="editContractForm" :rules="ContractFormRules" ref="editContractFormRef" label-width="80px">
        <!-- 姓名 -->
        <el-form-item label="姓名" prop="contactName">
          <el-input v-model="editContractForm.contactName"></el-input>
        </el-form-item>
        <el-form-item label="邮箱" prop="contactEmail">
          <el-input v-model="editContractForm.contactEmail"></el-input>
        </el-form-item>
        <el-form-item label="电话" prop="contactPhone">
          <el-input v-model="editContractForm.contactPhone"></el-input>
        </el-form-item>
        <el-form-item label="地址" prop="contactAddress">
          <el-input v-model="editContractForm.contactAddress"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editContractDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editContractInfo">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import BreadCrumb from '@/components/subcomponents/BreadCrumb.vue'
import request from '@/utils/request.js'
import { readFile, character, delay } from '@/utils/uploadExcel.js'
import xlsx from 'xlsx'
import { Loading } from 'element-ui'

export default {
  name: 'MyContractsBasicInfo',
  components: {
    BreadCrumb
  },
  data() {
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
      // 每页展示数据的条数
      pagesize: 6,
      // 当前展示的页码值
      currentpage: 1,
      // 当前数据的总数
      total: 0,
      querylist: {
        // 要查询的名字
        contactName: '',
        // 要查询的地址
        contactAddress: ''
      },
      // 存储联系人的数组
      contractslist: [],
      // 添加联系人的 Dialog 显示参数
      addContractsDialogVisible: false,
      // 添加联系人信息表单
      addContractForm: {
        contactName: '',
        contactEmail: '',
        contactPhone: '',
        contactAddress: ''
      },
      // 验证添加表单规则
      ContractFormRules: {
        contactName: [{ required: true, message: '请输入联系人姓名', trigger: 'blur' }],
        contactEmail: [
          { required: true, message: '请输入邮箱', trigger: 'blur' },
          { validator: checkEmail, trigger: 'blur' }
        ],
        contactPhone: [
          { required: true, message: '请输入电话', trigger: 'blur' },
          { validator: checkMobile, trigge: 'blur' }
        ],
        contactAddress: [{ required: true, message: '请输入联系人地址', trigger: 'blur' }]
      },
      // 修改联系人信息表单
      editContractForm: [],
      // 编辑联系人信息 Dialog 显示参数
      editContractDialogVisible: false,
      // 添加联系人标签页展示选项
      activeName: 'first',
      // 当前添加类型(单项、批量)
      addtype: 1,
      // 批量添加联系人数组
      tempData: [],
      submitButtonDisable: false,
      // 多选数组
      selectedData: [],
      // 批量导出按钮可点击属性
      addButtonDisable: false
    }
  },
  created() {
    // 从服务器获取数据
    this.getContractsList()
  },
  methods: {
    async getContractsList() {
      // 调用查询联系人接口，从服务器获取数据
      const { data: res } = await request.post('CusContactInfoes/find/' + this.pagesize + '/' + this.currentpage, this.querylist)
      // 判断获取是否成功
      if (res.status === 500) {
        this.contractslist = []
        return this.$message.error('没有联系人信息!')
      }
      if (res.status !== 200) return this.$message.error('获取联系人信息失败!')
      // 将数据保存到 contractslist 中
      this.contractslist = res.data
      this.total = res.total
    },
    // 监听查询输入框清空事件
    clearContracterName() {
      this.getContractsList()
    },
    // 监听查询地址输入框清空事件
    clearContracterAddress() {
      this.getContractsList()
    },
    // 通过搜索输入框搜索联系人
    findContractByName() {
      this.currentpage = 1
      this.getContractsList()
    },
    // 关闭添加对话框后重置表单验证
    addContractDialogClosed() {
      this.$refs.addContractFormRef.resetFields()
      this.submitButtonDisable = false
      this.activeName = 'first'
    },
    // 添加联系人信息
    addContractInfo() {
      if (this.addtype) {
        this.$refs.addContractFormRef.validate(async valid => {
          if (!valid) return
          // 调用添加联系人接口
          const { data: res } = await request.post('CusContactInfoes/', this.addContractForm)
          // 判断添加是否成功
          if (res.status !== 200) return this.$message.error('添加联系人信息失败!')
          // 关闭 addDialog 窗口
          this.addContractsDialogVisible = false
          // 重新获取联系人信息数据
          this.getContractsList()
          // 显示提示信息
          this.$message.success('添加联系人信息成功!')
        })
      } else {
        if (this.tempData <= 0) return this.$message.info('请先选择一个Excel文档')

        this.submitButtonDisable = true
        let loadingInstance = Loading.service({
          text: '玩命加载中',
          spinner: 'el-icon-loading'
        })

        let n = 0
        let send = async() => {
          if (n > this.tempData.length - 1) {
            this.submitButtonDisable = false
            loadingInstance.close()
            this.tempData.splice(0, this.tempData.length)
            // 关闭 addDialog 窗口
            this.addContractsDialogVisible = false
            // 重新获取联系人信息数据
            this.getContractsList()
            // 显示提示信息
            this.$message.success('添加联系人信息成功!')
            return
          }
          const body = this.tempData[n]
          const { data: res } = await request.post('CusContactInfoes/', body)
          if (res.status !== 200) return this.$message.error(`第${n++}条数据上传失败!`)
          n++
          send()
        }
        send()
      }
      this.submitButtonDisable = false
    },
    // 初始化添加用户单项添加中的表单数据
    handleClick(tab) {
      if (tab.index === '0') {
        this.addtype = 1
        this.tempData.splice(0, this.tempData.length)
        return
      }
      this.addContractDialogClosed()
      this.addtype = 0
    },
    // 监听编辑按钮点击事件
    async editContractsDialogShow(id) {
      // 调用获取修改用户信息接口
      const { data: res } = await request.get('CusContactInfoes/' + id)
      if (res.status !== 200) return this.$message.error('获取联系人信息失败!')
      this.editContractForm = res.data
      // 显示编辑 Diallog
      this.editContractDialogVisible = true
    },
    // 监听删除按钮点击事件
    async deleteContractInfo(id) {
      const confirmText = await this.$confirm('此操作将永久删除该联系人信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消删除')
      // 调用删除联系人信息接口
      const { data: res } = await request.delete('CusContactInfoes/' + id)
      // 判断删除是否成功，提示相关消息
      if (res.status !== 200) return this.$message.error('删除联系人信息失败!')
      this.$message.success('删除联系人信息成功!')
      this.currentpage = 1
      // 重新加载联系人数据
      this.getContractsList()
    },
    // 监听批量选择事件
    handleSelectionChange(val) {
      // 将选中的数据保存到 selectedData 中
      this.selectedData = val
    },
    // 监听批量删除按钮点击事件
    async deleteContractsInfo() {
      if (this.selectedData.length <= 0) return this.$message.info('请先选择数据')
      const confirmText = await this.$confirm('此操作将永久删除这些联系人信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消删除')
      this.submitButtonDisable = true
      let loadingInstance = Loading.service({
        text: '玩命加载中',
        spinner: 'el-icon-loading'
      })
      for (let index = 0; index < this.selectedData.length; index++) {
        // 调用删除联系人信息接口
        const { data: res } = await request.delete('CusContactInfoes/' + this.selectedData[index].contactId)
        if (res.status !== 200) {
          loadingInstance.close()
          return this.$message.error(`删除第${index++}个联系人信息失败!`)
        }
      }
      loadingInstance.close()
      this.$message.success('删除联系人信息成功!')
      this.querylist.contactName = ''
      this.querylist.contactAddress = ''
      this.currentpage = 1
      // 重新加载联系人数据
      this.getContractsList()
    },
    // 批量导出联系人到 Excel 表
    outputContacts() {
      if (this.selectedData.length <= 0) return this.$message.info('请先选择数据')
      this.addButtonDisable = true
      delay(100)
      let loadingInstance = Loading.service({
        text: '玩命加载中',
        spinner: 'el-icon-loading'
      })

      let arr = this.selectedData.map(item => {
        return {
          姓名: item.contactName,
          邮箱: item.contactEmail,
          电话: item.contactPhone,
          地址: item.contactAddress
        }
      })

      // JSON 数据转sheet型
      let sheet = xlsx.utils.json_to_sheet(arr)
      // 创建一个 Excel 表
      let book = xlsx.utils.book_new()
      xlsx.utils.book_append_sheet(book, sheet, 'sheet1')
      xlsx.writeFile(book, `user${new Date().getTime()}.xls`)

      delay(100)
      loadingInstance.close()
      this.addButtonDisable = false
      this.$message.success('导出联系人信息成功!')
      this.querylist.contactName = ''
      this.querylist.contactAddress = ''
      this.currentpage = 1
      // 重新加载联系人数据
      this.getContractsList()
    },
    // 改变每页显示条数
    handleSizeChange(pagesize) {
      this.pagesize = pagesize
      this.currentpage = 1
      // 调用获取联系人信息接口
      this.getContractsList()
    },
    // 改变显示页
    handleCurrentChange(val) {
      this.currentpage = val
      this.getContractsList()
    },
    // 关闭编辑对话框后重置表单验证
    editContractDialogClosed() {
      this.$refs.editContractFormRef.resetFields()
    },
    // 修改联系人信息
    editContractInfo() {
      this.$refs.editContractFormRef.validate(async valid => {
        if (!valid) return
        // 调用修改联系人接口
        const { data: res } = await request.put('CusContactInfoes/' + this.editContractForm.contactId, this.editContractForm)
        // 判断是否修改成功
        if (res.status !== 200) return this.$message.error('修改联系人信息失败!')
        // 若成功，关闭 Dialog 对话框
        this.editContractDialogVisible = false
        // 重新加载联系人数据
        this.getContractsList()
        // 提示修改成功
        this.$message.success('修改联系人信息成功!')
      })
    },
    // 获取 Excel 表格数据
    async handle(ev) {
      let file = ev.raw
      if (!file) return

      this.addContactesShow = false
      let loadingInstance = Loading.service({
        text: '玩命加载中',
        spinner: 'el-icon-loading'
      })

      await delay(100)
      // 获取数据并转为 JSON 格式
      let data = await readFile(file)
      let workbook = xlsx.read(data, { type: 'binary' })
      let worksheet = workbook.Sheets[workbook.SheetNames[0]]
      data = xlsx.utils.sheet_to_json(worksheet)
      // 将获取的数据转换成对应的字段名，以传递给服务器 ( 姓名: contactName, 邮箱: contactEmail, 电话: contactPhone, 地址: contactAddress )
      let arr = []
      data.forEach(item => {
        const obj = {}
        for (let i in character) {
          if (!Object.prototype.hasOwnProperty.call(character, i)) break
          let v = character[i]
          let text = v.text
          let type = v.type
          v = item[text] || ''
          if (type === 'string') v = String(v)
          obj[i] = v
        }
        arr.push(obj)
      })
      await delay(100)
      this.tempData = arr
      this.addContactesShow = true
      loadingInstance.close()
    }
  }
}
</script>

<style lang="less" scoped>
.buttonBox {
  width: 100%;
  height: 95px;
  .el-alert {
    margin-top: 10px;
  }
}
.tableBox {
  max-height: 400px;
  overflow-x: hidden;
  overflow-y: scroll;
}
</style>
