<template>
<!--投诉模块-->
  <div>
    <!-- 面包屑区域 -->
    <BreadCrumb firstname="客户关系" secondname="投诉建议处理"></BreadCrumb>
    <!-- 搜索区域 -->
    <el-card style="margin-bottom: 15px">
      <el-row :gutter="20">
        <!-- 投诉人输入框 -->
        <el-col :span="4">
          <el-input v-model="querylist.customerName" placeholder="投诉人姓名" clearable @clear="clearAdvicer"></el-input>
        </el-col>
        <!-- 投诉建议输入框 -->
        <el-col :span="6">
          <el-input v-model="querylist.complaintId" placeholder="投诉建议 id" clearable @clear="clearAdvice"></el-input>
        </el-col>
        <!-- 查询按钮 -->
        <el-col :span="12">
          <el-button @click="findAdviceByName" type="primary">查询</el-button>
        </el-col>
        <!-- 添加按钮 -->
        <el-col :span="1">
          <el-button @click="addAdviseDialogVisible = true" type="primary">添加</el-button>
        </el-col>
      </el-row>
    </el-card>
    <!-- 投诉建议列表区域 -->
    <el-card>
      <!-- 投诉建议列表 -->
      <el-table :data="adviselist">
        <!-- <el-table :data="userlist.slice((currentPage-1)*pagesize,currentPage*pagesize)"> -->
        <!-- 建议编号 -->
        <el-table-column prop="complaintId" label="投诉建议id" sortable width="120"> </el-table-column>
        <!-- 客户编号 -->
        <el-table-column prop="customerName" label="客户" sortable width="110"> </el-table-column>
        <!-- 内容 -->
        <el-table-column prop="complaintInfo" label="内容" width="320">
          <!-- 将内容进行渲染，限制内容的长度，多余部分用省略号代替 -->
          <template slot-scope="scope">
            <div class="line-limit-length">{{ scope.row.complaintInfo }}</div>
          </template>
        </el-table-column>
        <!-- 类型 -->
        <el-table-column prop="complaintStyle" label="类型" width="120">
          <!-- 将类型进行渲染，用不同颜色的标签显示不同的类型 -->
          <template slot-scope="scope">
            <el-tag size="small" :type="scope.row.complaintStyle === 0 ? 'success' : 'danger'">{{ scope.row.complaintStyle === 0 ? '建议' : '投诉' }}</el-tag>
          </template>
        </el-table-column>
        <!-- 发布时间 -->
        <el-table-column prop="complaintTime" label="发布时间" width="140">
          <template slot-scope="scope">
            {{ scope.row.complaintTime | initDate }}
          </template>
        </el-table-column>
        <!-- 状态 -->
        <el-table-column prop="complaintState" label="状态" width="120">
          <!-- 将状态进行渲染，用不同颜色的标签显示不同的状态 -->
          <template slot-scope="scope">
            <el-tag size="small" :type="scope.row.complaintState === 0 ? 'info' : scope.row.complaintState === 1 ? 'warning' : 'success'">{{
              scope.row.complaintState === 0 ? '未处理' : scope.row.complaintState === 1 ? '正在处理中' : '已处理'
            }}</el-tag>
          </template>
        </el-table-column>
        <!-- 处理人 -->
        <el-table-column prop="salesManName" label="处理人" width="120">
          <!-- 将处理人进行渲染，若为空，则显示 '-' -->
          <template slot-scope="scope">
            {{ scope.row.salesManName === null ? '-' : scope.row.salesManName }}
          </template>
        </el-table-column>
        <!-- 处理时间 -->
        <el-table-column prop="complaintHandleTime" label="处理时间" width="120">
          <!-- 将处理时间进行渲染，若时间为空，则显示 '-' -->
          <template slot-scope="scope">
            {{ scope.row.complaintHandleTime === null ? '-' : scope.row.complaintHandleTime | initDate }}
          </template>
        </el-table-column>
        <!-- 操作 -->
        <el-table-column fixed="right" label="操作" width="180">
          <template slot-scope="scope">
            <!-- 查看信息 -->
            <el-button @click="adviceInfoDialogShow(scope.row.complaintId)" type="primary" icon="el-icon-view" size="small"></el-button>
            <!-- 编辑信息 -->
            <el-button @click="editAdviceDialogShow(scope.row.complaintId)" type="warning" icon="el-icon-edit" size="small" :disabled="scope.row.complaintState === 2 ? true : false"></el-button>
            <!-- 删除信息 -->
            <el-button @click="deleteAdviceInfo(scope.row.complaintId)" type="danger" icon="el-icon-delete" size="small"></el-button>
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
        :total="advicetotal"
      >
      </el-pagination>
    </el-card>
    <!-- 查看建议信息弹窗 -->
    <el-dialog title="投诉建议" :visible.sync="adviceDialogVisible" width="50%" center>
      <!-- 建议属性区域 -->
      <el-descriptions border :column="2">
        <!-- 投诉人 -->
        <el-descriptions-item label="投诉人">{{ adviceinfo.customerName }}</el-descriptions-item>
        <!-- 发布时间 -->
        <el-descriptions-item label="发布时间">{{ adviceinfo.complaintTime | initDate }}</el-descriptions-item>
        <!-- 投诉类型 -->
        <el-descriptions-item label="类型">
          <el-tag size="small" :type="adviceinfo.complaintStyle === 0 ? 'success' : 'danger'">{{ adviceinfo.complaintStyle === 0 ? '建议' : '投诉' }}</el-tag>
        </el-descriptions-item>
        <!-- 状态 -->
        <el-descriptions-item label="状态">
          <el-tag size="small" :type="adviceinfo.complaintState === 0 ? 'info' : adviceinfo.complaintState === 1 ? 'warning' : 'success'">{{
            adviceinfo.complaintState === 0 ? '未处理' : adviceinfo.complaintState === 1 ? '正在处理中' : '已处理'
          }}</el-tag>
        </el-descriptions-item>
        <!-- 处理人 -->
        <el-descriptions-item label="处理人" v-if="adviceinfo.complaintState">{{ adviceinfo.salesManName }}</el-descriptions-item>
        <!-- 处理时间 -->
        <el-descriptions-item label="处理时间" v-if="adviceinfo.complaintState">{{ adviceinfo.complaintHandleTime | initDate }}</el-descriptions-item>
      </el-descriptions>
      <el-divider></el-divider>
      <!-- 建议内容区域 -->
      <el-descriptions direction="vertical" border>
        <el-descriptions-item :label="adviceinfo.complaintStyle === 0 ? '建议内容' : '投诉内容'">
          <div style="text-indent: 2em">{{ adviceinfo.complaintInfo }}</div>
        </el-descriptions-item>
      </el-descriptions>
      <el-divider v-if="adviceinfo.complaintState === 2"></el-divider>
      <!-- 处理结果区域 -->
      <el-descriptions direction="vertical" border :column="1">
        <!-- 处理完成日期 -->
        <el-descriptions-item label="处理完成日期" v-if="adviceinfo.complaintState === 2">
          <div style="text-indent: 2em">{{ adviceinfo.complaintOverTime | initDate }}</div>
        </el-descriptions-item>
        <!-- 处理结果 -->
        <el-descriptions-item label="处理结果" v-if="adviceinfo.complaintState === 2">
          <div style="text-indent: 2em">{{ adviceinfo.complaintResult === null ? '无' : adviceinfo.complaintResult }}</div>
        </el-descriptions-item>
        <!-- 处理说明 -->
        <el-descriptions-item label="处理说明" v-if="adviceinfo.complaintState === 2">
          <div style="text-indent: 2em">{{ adviceinfo.complaintExplain === null ? '无' : adviceinfo.complaintExplain }}</div>
        </el-descriptions-item>
      </el-descriptions>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="adviceDialogVisible = false">返 回</el-button>
      </span>
    </el-dialog>
    <!-- 修改投诉建议状态弹窗 -->
    <el-dialog title="修改状态" :visible.sync="editAdviceDialogVisible" width="50%" @close="editAdviceDialogClosed" center>
      <el-form :model="editAdviceFrom" :rules="editAdviceFormRules" ref="editAdviceFormRef" label-width="100px">
        <!-- 当前状态选项 -->
        <el-form-item label="当前状态">
          <el-select v-model="editAdviceFrom.complaintState" placeholder="请选择" @change="changeEditAble" :disabled="selectAble === 2 ? true : false">
            <el-option v-for="item in statusOptions" :key="item.value" :label="item.label" :value="item.value"> </el-option>
          </el-select>
        </el-form-item>
        <!-- 处理人姓名 -->
        <el-form-item label="处理人 id" :prop="editAdviceFrom.complaintState === 0 ? null : 'salesManId'">
          <el-input v-model="editAdviceFrom.salesManId" :disabled="editAdviceFrom.complaintState === 0 ? true : false"></el-input>
        </el-form-item>
        <!-- 处理日期 -->
        <el-form-item label="处理时间" :prop="editAdviceFrom.complaintState === 0 ? null : 'complaintHandleTime'">
          <el-date-picker v-model="editAdviceFrom.complaintHandleTime" type="date" placeholder="选择日期" :disabled="editAdviceFrom.complaintState === 0 ? true : false"> </el-date-picker>
        </el-form-item>
        <!-- 完成处理日期 -->
        <el-form-item label="完成日期" :prop="editAdviceFrom.complaintState === 2 ? 'complaintOverTime' : null">
          <el-date-picker v-model="editAdviceFrom.complaintOverTime" type="date" placeholder="选择日期" :disabled="editAdviceFrom.complaintState === 2 ? false : true"> </el-date-picker>
        </el-form-item>
        <!-- 处理结果 -->
        <el-form-item label="处理结果" :prop="editAdviceFrom.complaintState === 2 ? 'complaintResult' : null">
          <el-input type="textarea" :rows="3" v-model="editAdviceFrom.complaintResult" :disabled="editAdviceFrom.complaintState === 2 ? false : true"></el-input>
        </el-form-item>
        <!-- 处理说明 -->
        <el-form-item label="处理说明">
          <el-input type="textarea" :rows="3" v-model="editAdviceFrom.complaintExplain" :disabled="editAdviceFrom.complaintState === 2 ? false : true"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editAdviceDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editAdviceInfo">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 添加投诉信息弹窗 -->
    <el-dialog title="添加投诉建议信息" :visible.sync="addAdviseDialogVisible" width="50%" center @close="addAdviceDialogClosed">
      <!-- 主体内容区域 -->
      <el-form :model="addAdviceForm" :rules="editAdviceFormRules" ref="addAdviceFormRef" label-width="90px">
        <!-- 姓名 -->
        <el-form-item label="投诉人 id" prop="customerId">
          <el-input v-model="addAdviceForm.customerId"></el-input>
        </el-form-item>
        <!-- 类型 -->
        <el-form-item label="类型" prop="complaintStyle">
          <el-radio-group v-model="addAdviceForm.complaintStyle" size="small">
            <el-radio :label="0" border>建议</el-radio>
            <el-radio :label="1" border>投诉</el-radio>
          </el-radio-group>
        </el-form-item>
        <!-- 电话 -->
        <el-form-item label="内容" prop="complaintInfo">
          <el-input type="textarea" :rows="4" placeholder="请输入内容" v-model="addAdviceForm.complaintInfo"> </el-input>
        </el-form-item>
      </el-form>
      <!-- 底部按钮区域 -->
      <span slot="footer" class="dialog-footer">
        <el-button @click="addAdviseDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addAdviceInfo">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import BreadCrumb from '@/components/subcomponents/BreadCrumb.vue'
import request from '@/utils/request.js'
import { initDate } from '@/filters'

export default {
  name: 'MyAdviceHandle',
  components: {
    BreadCrumb
  },
  filters: {
    initDate
  },
  data() {
    return {
      adviselist: [],
      currentPage: 1,
      pagesize: 6,
      advicetotal: 0,
      querylist: {
        complaintId: '',
        customerName: ''
      },
      addAdviceForm: {
        customerId: '',
        complaintTime: '',
        complaintStyle: 0,
        complaintState: 0,
        complaintInfo: ''
      },
      addAdviseDialogVisible: false,
      adviceDialogVisible: false,
      adviceinfo: {},
      editAdviceDialogVisible: false,
      editAdviceFrom: {},
      editAble: 0,
      selectAble: 0,
      processing: true,
      solved: true,
      statusOptions: [
        { value: 0, label: '未处理' },
        { value: 1, label: '正在处理中' },
        { value: 2, label: '已处理' }
      ],
      editAdviceFormRules: {
        customerId: [{ required: true, message: '请输入投诉人', trigger: 'blur' }],
        salesManId: [{ required: true, message: '请输入处理人', trigger: 'blur' }],
        complaintTime: [{ required: true, message: '请选择开始处理时间', trigger: 'blur' }],
        complaintHandleTime: [{ required: true, message: '请选择开始处理时间', trigger: 'blur' }],
        complaintOverTime: [{ required: true, message: '请选择完成处理时间', trigger: 'blur' }],
        complaintResult: [{ required: true, message: '请输入处理结果', trigger: 'blur' }],
        complaintInfo: [{ required: true, message: '请输入投诉内容', trigger: 'blur' }]
      }
    }
  },
  created() {
    this.getAdviseList()
  },
  methods: {
    // 调用服务器接口获取投诉建议数据
    async getAdviseList() {
      // 调用接口
      const { data: res } = await request.post('Complaints/find/' + this.pagesize + '/' + this.currentPage, this.querylist)
      // 判断获取是否成功
      if (res.status !== 200) this.$message.danger('获取数据失败!')
      // 将数据保存到 adviselist 中
      this.adviselist = res.data
      this.advicetotal = res.total
    },
    // 监听清空建议 id 输入框事件
    clearAdvice() {
      this.getAdviseList()
    },
    // 监听清空投诉人输入框事件
    clearAdvicer() {
      this.getAdviseList()
    },
    // 监听查询按钮点击事件
    async findAdviceByName() {
      // 调用接口获取数据
      this.getAdviseList()
    },
    // 监听添加 Dialog 关闭事件
    addAdviceDialogClosed() {
      this.$refs.addAdviceFormRef.resetFields()
    },
    // 监听添加确定按钮点击事件
    addAdviceInfo() {
      this.$refs.addAdviceFormRef.validate(async valid => {
        if (!valid) return
        // 给发布时间赋值
        this.addAdviceForm.complaintTime = new Date().toLocaleDateString()
        // 调用添加投诉状态接口
        const { data: res } = await request.post('Complaints', this.addAdviceForm)
        // 判断添加是否成功
        if (res.status !== 200) return this.$message.error('添加投诉信息失败!')
        // 关闭 addDialog 窗口
        this.addAdviseDialogVisible = false
        // 重新获取投诉建议信息数据
        this.getAdviseList()
        // 显示提示信息
        this.$message.success('添加投诉建议信息成功!')
      })
    },
    // 监听查看按钮点击事件
    async adviceInfoDialogShow(id) {
      // 调用服务器接口获取数据
      const { data: res } = await request.get('Complaints/' + id)
      // 判断获取是否成功
      if (res.status !== 200) return this.$message.error('获取信息失败!')
      // 将数据保存到 adviceinfo 中
      this.adviceinfo = res.data
      this.adviceDialogVisible = true
    },
    // 监听修改按钮点击事件
    async editAdviceDialogShow(id) {
      // 调用服务器接口获取数据
      const { data: res } = await request.get('Complaints/' + id)
      // 判断获取是否成功
      if (res.status !== 200) return this.$message.error('获取信息失败!')
      // 将数据保存到 adviceinfo 中
      this.editAdviceFrom = res.data
      this.selectAble = this.editAdviceFrom.complaintState
      this.editAdviceDialogVisible = true
    },
    // 监听删除按钮点击事件
    async deleteAdviceInfo(id) {
      const confirmText = await this.$confirm('此操作将永久删除该建议信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return this.$message.info('已取消删除')
      // 调用删除用户信息接口
      const { data: res } = await request.delete('Complaints/' + id)
      // 判断删除是否成功，提示相关消息
      if (res.status !== 200) return this.$message.error('删除信息失败!')
      this.$message.success('删除建议信息成功!')
      // 重新加载用户数据
      this.getAdviseList()
    },
    // 动态为编辑框添加可编辑属性
    changeEditAble(newVal) {
      this.editAdviceFrom.complaintState = newVal
    },
    // 监听关闭编辑框事件
    editAdviceDialogClosed() {
      this.$refs.editAdviceFormRef.resetFields()
    },
    // 监听确定修改按钮事件
    editAdviceInfo() {
      this.$refs.editAdviceFormRef.validate(async valid => {
        if (!valid) return
        // 调用修改投诉状态接口
        const { data: res } = await request.put('Complaints/' + this.editAdviceFrom.complaintId, this.editAdviceFrom)
        // 判断修改是否成功
        if (res.status !== 200) return this.$message.error('修改投诉信息失败!')
        // 关闭 addDialog 窗口
        this.editAdviceDialogVisible = false
        // 重新获取联系人信息数据
        this.getAdviseList()
        // 显示提示信息
        this.$message.success('修改投诉建议信息成功!')
      })
    },
    // 改变每页显示条数
    handleSizeChange(pagesize) {
      this.pagesize = pagesize
      this.currentPage = 1
      // 调用服务器接口，以当前 pagesize 值重新获取每页内容
      this.getAdviseList()
    },
    // 改变显示页
    handleCurrentChange(val) {
      this.currentPage = val
      // 调用服务器接口，以当前 currentPage 值重新获取每页内容
      this.getAdviseList()
    }
  }
}
</script>

<style lang="less" scoped>
</style>
