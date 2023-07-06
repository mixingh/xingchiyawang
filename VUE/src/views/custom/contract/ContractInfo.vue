<template>
  <!-- 合约信息组件 -->
  <div>
    <!-- 面包屑区域 -->
    <BreadCrumb firstname="客户" secondname="合约信息"></BreadCrumb>
    <!-- 搜索区域 -->
    <el-card ref="SearchCard" :style="searchcardcss">
      <el-row :gutter="20">
        <!-- 合同名称输入框 -->
        <el-col :span="6">
          <el-input v-model="querylist.contractname" placeholder="合同名称" @keydown="clearName" clearable @clear="clearName"></el-input>
        </el-col>
        <!-- 销售员姓名输入框 -->
        <el-col :span="4">
          <el-input v-model="querylist.salesname" placeholder="销售员姓名" clearable @clear="clearName"></el-input>
        </el-col>
        <!-- 客户姓名输入框 -->
        <el-col :span="4">
          <el-input v-model="querylist.customname" placeholder="客户姓名" clearable @clear="clearName"></el-input>
        </el-col>
        <!-- 查询按钮 -->
        <el-col :span="6">
          <el-button @click="findContractsByName" type="primary">查询</el-button>
        </el-col>
        <!-- 筛选条件 -->
        <el-col :span="3">
          <el-select style="width: 120px" @change="handleCommand" v-model="value" placeholder="请选择">
            <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value"> </el-option>
          </el-select>
        </el-col>
      </el-row>
    </el-card>
    <!-- 合约信息区域 -->
    <div class="contract-container">
      <div class="contractdiv addcontractdiv" @click="addContractDialogVisible = true">
        <img src="@/assets/addButton.png" alt="" />
      </div>
      <div v-for="item in screenList" :key="item.contractId" class="contractdiv" @click="showContract(item.contractId)">
        <!-- 合同状态、编号和名称 -->
        <dl>
          <dt class="statusdt">
            <el-tag size="mini" :type="item.contractState === 0 ? 'danger' : item.contractState === 1 ? 'success' : 'info'">{{
              item.contractState === 0 ? '未签订' : item.contractState === 1 ? '已签订' : '已失效'
            }}</el-tag>
            <span class="uspan">{{ item.contractId }}</span>
          </dt>
          <dt class="contractname line-limit-length">{{ item.contractName ? `《${item.contractName}》` : '《合同名称》' }}</dt>
        </dl>
        <!-- 合同金额 -->
        <table>
          <tbody>
            <tr>
              <td style="width: 50%"><span class="uspan">合同金额</span></td>
            </tr>
            <tr>
              <td>
                <span class="amout">￥{{ item.contractAmount | numberToCurrency }}</span>
              </td>
            </tr>
          </tbody>
        </table>
        <!-- 签单人和日期 -->
        <table>
          <tbody>
            <tr>
              <td style="width: 50%"><span class="uspan">签单人</span></td>
              <td><span class="uspan">合同生效日期</span></td>
            </tr>
            <tr>
              <td>
                <span v-if="item.signerName">{{ item.signerName }}</span>
                <span v-else>-</span>
              </td>
              <td>
                <span v-if="item.contractStartDate">{{ item.contractStartDate | initDate }}</span>
                <span v-else>-</span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <!-- 添加合约信息弹窗 -->
    <el-dialog title="添加合约信息" :visible.sync="addContractDialogVisible" width="50%" center @close="addContractDialogClosed">
      <!-- 主体内容区域 -->
      <el-form :model="addContractForm" :rules="ContractFormRules" ref="addContractFormRef" label-width="100px">
        <!-- 名称 -->
        <el-form-item label="合同名称" prop="contractName">
          <el-input v-model="addContractForm.contractName"></el-input>
        </el-form-item>
        <el-form-item label="合同金额" prop="contractAmount">
          <el-input v-model="addContractForm.contractAmount"></el-input>
        </el-form-item>
        <el-form-item label="状态">
          <el-radio-group v-model="addContractForm.contractState">
            <el-radio :label="0">未签订</el-radio>
            <el-radio :label="1">已签订</el-radio>
            <el-radio :label="2">已失效</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="签单人 id" :prop="addContractForm.contractState === 0 ? '' : 'singerId'">
          <el-input v-model="addContractForm.singerId" :disabled="addContractForm.contractState === 0 ? true : false"></el-input>
        </el-form-item>
        <el-form-item label="合同有效期" :prop="addContractForm.contractState === 0 ? '' : 'contractStartDate'">
          <el-date-picker v-model="dateTime" value-format="yyyy-MM-dd hh:mm:ss" :disabled="addContractForm.contractState === 0 ? true : false" type="datetimerange" placeholder="选择日期"> </el-date-picker>
        </el-form-item>
        <el-form-item label="合同内容" prop="contractInfo">
          <el-input type="textarea" :rows="4" placeholder="请输入合同内容" v-model="addContractForm.contractInfo"> </el-input>
        </el-form-item>
      </el-form>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="addContractDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addContractInfo">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 查看合约信息弹窗 -->
    <el-dialog title="合约详细信息" :visible.sync="contractInfoDialogVisible" width="75%" center>
      <!-- 修改合约信息弹窗 -->
      <el-dialog width="60%" title="修改合约信息" :visible.sync="editDialogShow" append-to-body @close="editContractDialogClosed">
        <el-form :model="editContractForm" :rules="ContractFormRules" ref="editContractFormRef" label-width="100px">
          <!-- 名称 -->
          <el-form-item label="合同名称" prop="contractName">
            <el-input v-model="editContractForm.contractName"></el-input>
          </el-form-item>
          <el-form-item label="合同金额" prop="contractAmount">
            <el-input v-model="editContractForm.contractAmount"></el-input>
          </el-form-item>
          <el-form-item label="状态">
            <el-radio-group v-model="editContractForm.contractState">
              <el-radio :label="0">未签订</el-radio>
              <el-radio :label="1">已签订</el-radio>
              <el-radio :label="2">已失效</el-radio>
            </el-radio-group>
          </el-form-item>
          <el-form-item label="签单人 id" :prop="editContractForm.contractState ? 'singerId' : ''">
            <el-input v-model="editContractForm.singerId" :disabled="editContractForm.contractState === 0 ? true : false"></el-input>
          </el-form-item>
          <el-form-item label="合同有效期" :prop="editContractForm.contractState ? 'contractStartDate' : ''">
            <el-date-picker v-model="dateTime" value-format="yyyy-MM-dd hh:mm:ss" :disabled="editContractForm.contractState === 0 ? true : false" type="datetimerange" placeholder="选择日期">
            </el-date-picker>
          </el-form-item>
          <el-form-item label="合同内容" prop="contractInfo">
            <el-input type="textarea" :rows="4" placeholder="请输入合同内容" v-model="editContractForm.contractInfo"> </el-input>
          </el-form-item>
        </el-form>
        <span slot="footer" class="dialog-footer">
          <el-button @click="editDialogShow = false">取 消</el-button>
          <el-button type="primary" @click="editContractInfo">确 定</el-button>
        </span>
      </el-dialog>
      <!-- 合同信息区域 -->
      <el-descriptions title="操作" border :column="1">
        <template slot="extra">
          <el-button type="warning" size="small" @click="showEditContractDialog">修改</el-button>
          <el-button type="danger" size="small" @click="deleteContract">删除</el-button>
        </template>
        <el-descriptions-item label="合同编号" width="200px">{{ contractinfo.contractId }}</el-descriptions-item>
        <el-descriptions-item label="合同名称">{{ contractinfo.contractName }}</el-descriptions-item>
      </el-descriptions>
      <el-descriptions border :column="2">
        <el-descriptions-item label="合同金额">￥{{ contractinfo.contractAmount | numberToCurrency }} </el-descriptions-item>
        <el-descriptions-item label="状态">
          <el-tag size="small" :type="contractinfo.contractState === 0 ? 'danger' : contractinfo.contractState === 1 ? 'success' : 'info'">{{
            contractinfo.contractState === 0 ? '未签订' : contractinfo.contractState === 1 ? '已签订' : '已失效'
          }}</el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="签单人" v-if="contractinfo.contractState !== 0">{{ contractinfo.signerName }}</el-descriptions-item>
        <el-descriptions-item label="签单日期" v-if="contractinfo.contractState !== 0">{{ contractinfo.contractStartDate ? contractinfo.contractStartDate : '-' | initDate }}</el-descriptions-item>
        <el-descriptions-item label="合同生效日期" v-if="contractinfo.contractState !== 0">{{ contractinfo.contractStartDate ? contractinfo.contractStartDate : '-' | initDate }}</el-descriptions-item>
        <el-descriptions-item label="合同失效日期" v-if="contractinfo.contractState !== 0">{{ contractinfo.contractEndDate ? contractinfo.contractEndDate : '-' | initDate }}</el-descriptions-item>
      </el-descriptions>
      <!-- 合同内容区域 -->
      <div class="contract-text">
        <!-- 合同标题 -->
        <div class="contract-title">{{ contractinfo.contractName }}</div>
        <!-- 合同正文 -->
        <div class="contract-body">{{ contractinfo.contractInfo ? contractinfo.contractInfo : '无' }}</div>
      </div>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="contractInfoDialogVisible = false">返 回</el-button>
      </span>
    </el-dialog>
    <!-- 返回顶部按钮 -->
    <el-backtop icon="el-icon-caret-top"> </el-backtop>
  </div>
</template>

<script>
import BreadCrumb from '@/components/subcomponents/BreadCrumb.vue'
import { numberToCurrency, initDate } from '@/filters'
import request from '@/utils/request.js'

export default {
  name: 'MyContractInfo',
  components: {
    BreadCrumb
  },
  filters: {
    numberToCurrency,
    initDate
  },
  data() {
    return {
      // 合同对象
      contractslist: [],
      currentPage: 1,
      pageSize: 6,
      // 查找对象
      querylist: {
        contractId: '',
        contractName: ''
      },
      // 顶部导航栏 CSS 样式
      searchcardcss: 'margin-bottom: 15px',
      // 添加合约信息 Dialog 显示参数
      addContractDialogVisible: false,
      addContractForm: {
        singerId: 0,
        contractName: '',
        contractAmount: 0,
        contractState: 0,
        contractStartDate: '',
        contractEndDate: '',
        contractInfo: ''
      },
      // 查看合约信息 Dialog 显示参数
      contractInfoDialogVisible: false,
      // 合约具体信息对象
      contractinfo: [],
      // 修改合约信息 Dialog 显示参数
      editDialogShow: false,
      editContractForm: {},
      // 验证修改信息规则
      ContractFormRules: {
        contractName: [{ required: true, message: '请输入合同名称', trigger: 'blur' }],
        contractAmount: [{ required: true, message: '请输入合同金额', trigger: 'blur' }],
        singerId: [
          { required: true, message: '请输入签单人 id', trigger: 'blur' }
        ],
        contractStartDate: [{ required: true, message: '请输入签单日期', trigger: 'blur' }],
        contractInfo: [{ required: true, message: '请输入合同内容', trigger: 'blur' }]
      },
      value: '全部类型',
      options: [
        { value: 'all', label: '全部类型' },
        { value: 'error', label: '未签订' },
        { value: 'right', label: '已签订' },
        { value: 'overtime', label: '已失效' }
      ],
      // 用于筛选的数组
      screenList: [],
      dateTime: ''
    }
  },
  created() {
    this.getContractsList()
  },
  mounted() {
    // 监听页面滚动事件
    window.addEventListener('scroll', this.showSearch)
  },
  methods: {
    // 调用接口从服务器获取数据
    async getContractsList() {
      // 调用获取数据接口
      const { data: res } = await request.post('CusContractInfoes/find/1024/1', this.querylist)
      // 判断获取是否成功
      if (res.status === 500) {
        this.userlist = []
        return this.$message.error('没有用户信息!')
      }
      if (res.status !== 200) return this.$message.error('获取服务器数据失败')
      // 将数据保存到 contractslist 中
      this.contractslist = res.data
      this.screenList = this.contractslist
    },
    // 监听合同输入框清空事件
    clearName() {
      this.getContractsList()
    },
    // 监听搜索按钮点击事件
    findContractsByName() {
      this.getContractsList()
    },
    // 监听筛选框点击事件
    handleCommand(command) {
      switch (command) {
        case 'all':
          this.screenList = this.contractslist
          break
        case 'error':
          this.screenList = this.contractslist.filter(item => item.contractState === 0)
          break
        case 'right':
          this.screenList = this.contractslist.filter(item => item.contractState === 1)
          break
        case 'overtime':
          this.screenList = this.contractslist.filter(item => item.contractState === 2)
          break
        default:
          break
      }
    },
    // 显示合同详细信息按钮
    async showContract(id) {
      // 调用获取合同信息接口
      const { data: res } = await request.get('CusContractInfoes/' + id)
      // 判断获取是否成功
      if (res.status !== 200) return this.$message.error('获取合同信息失败!')
      this.contractinfo = res.data
      this.contractInfoDialogVisible = true
    },
    // 当添加合同信息的签单人 Id 输入后，判断该 Id 是否存在
    // 监听添加合同信息确定按钮点击事件
    async addContractInfo() {
      this.$refs.addContractFormRef.validate(async valid => {
        if (!valid) return
        // 调用添加联系人接口
        this.addContractForm.contractStartDate = this.dateTime[0]
        this.addContractForm.contractEndDate = this.dateTime[1]
        const { data: res } = await request.post('CusContractInfoes/', this.addContractForm)
        // 判断添加是否成功
        if (res.status !== 200) return this.$message.error('添加合同信息失败!')
        // 关闭 addDialog 窗口
        this.addContractForm.singerId = 0
        this.addContractForm.contractStartDate = ''
        this.addContractForm.contractEndDate = ''
        this.addContractDialogVisible = false
        // 重新获取联系人信息数据
        this.getContractsList()
        // 显示提示信息
        this.$message.success('添加合同信息成功!')
      })
    },
    // 关闭添加合约信息重置验证
    addContractDialogClosed() {
      this.$refs.addContractFormRef.resetFields()
      this.addContractForm.contractState = 0
    },
    // 固定顶部搜索栏
    showSearch() {
      // 获取当前滚动条向下滚动的距离
      const scrollTop = document.documentElement.scrollTop || document.body.scrollTop
      // 当页面滚动到高度300px处时，显示搜索框
      if (scrollTop > 300) {
        this.searchcardcss = 'margin-bottom: 15px;position: fixed;top: 0;width: 100%;'
      } else {
        this.searchcardcss = 'margin-bottom: 15px'
      }
    },
    // 监听修改按钮点击事件
    async showEditContractDialog() {
      const { data: res } = await request.get('CusContractInfoes/' + this.contractinfo.contractId)
      if (res.status !== 200) return this.$message.error('获取合同信息失败!')
      this.editContractForm = res.data
      this.dateTime = [new Date(this.editContractForm.contractStartDate), new Date(this.editContractForm.contractEndDate)]
      this.editDialogShow = true
    },
    // 关闭修改合约信息重置验证
    editContractDialogClosed() {
      this.$refs.editContractFormRef.resetFields()
    },
    // 监听修改确定按钮点击事件
    async editContractInfo() {
      this.$refs.editContractFormRef.validate(async valid => {
        if (!valid) return
        this.editContractForm.contractStartDate = this.dateTime[0]
        this.editContractForm.contractEndDate = this.dateTime[1]
        if (this.editContractForm.contractState === 0) {
          this.editContractForm.signerName = ''
          this.editContractForm.contractStartDate = ''
          this.editContractForm.contractEndDate = ''
        }
        // 调用修改联系人接口
        const { data: res } = await request.put('CusContractInfoes/' + this.editContractForm.contractId, this.editContractForm)
        // 判断是否修改成功
        if (res.status !== 200) return this.$message.error('修改合同信息失败!')
        // 若成功，关闭 Dialog 对话框
        this.editDialogShow = false
        // 重新加载合同数据
        this.showContract(this.editContractForm.contractId)
        this.getContractsList()
        // 提示修改成功
        this.$message.success('修改合同信息成功!')
      })
    },
    // 监听删除合约信息按钮
    async deleteContract() {
      const confirmText = await this.$confirm('此操作将永久删除该合同信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消删除')
      // 调用删除合同信息接口
      const { data: res } = await request.delete('CusContractInfoes/' + this.contractinfo.contractId)
      // 判断删除是否成功
      if (res.status !== 200) return this.$message.error('删除合同信息失败!')
      this.$message.success('删除合同信息成功!')
      this.contractInfoDialogVisible = false
      // 重新获取数据
      this.getContractsList()
    }
  }
}
</script>

<style lang="less" scoped>
.contract-container {
  width: 100%;
  height: auto;
}
.contractdiv {
  height: 260px;
  width: 340px;
  padding: 5px 25px;
  margin: 0 40px;
  margin-top: 15px;
  margin-bottom: 20px;
  background-color: white;
  border-radius: 5px;
  display: inline-block;
  cursor: pointer;
  .statusdt {
    margin-bottom: 10px;
  }
  .contractname {
    font-size: 23px;
    color: #000;
    font-weight: 500;
  }
  > table {
    width: 100%;
    margin-bottom: 10px;
  }
  > dl {
    margin-bottom: 30px;
  }
  > span {
    font-family: 'PingFang SC', 'Gotham', 'Myriad Set Pro', 'STHeitiSC-Light', 'Microsoft YaHei', sans-serifsans-serif;
  }
  .uspan {
    color: #666;
  }
  .amout {
    font-size: 25px;
    color: #fc4e49;
    font-weight: bold;
  }
}
.contractdiv:hover {
  background-color: #d9ecff;
  box-shadow: 5px 5px 5px 0 rgba(0, 0, 0, 0.05);
}
.addcontractdiv {
  float: left;
  > img {
    position: relative;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    text-align: center;
  }
}
.contract-text {
  margin-top: 40px;
  border: 1px solid #ebeef5;
  width: 100%;
  height: auto;
  padding: 10px;
  box-sizing: border-box;
  .contract-title {
    width: 100%;
    text-align: center;
    line-height: 35px;
    font-size: 20px;
    color: black;
  }
  .contract-body {
    width: 100%;
    font-size: 15px;
    line-height: 25px;
    color: #666666;
    text-indent: 2em;
  }
}
</style>
