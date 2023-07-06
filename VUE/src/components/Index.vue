<template>
  <div>
    <el-row style="margin-bottom: 15px">
      <el-col :span="24">
        <el-card>
          <el-row>
            <el-col :span="2" style="text-align: center">
              <el-avatar :size="50" :src="image" class="img"></el-avatar>
            </el-col>
            <el-col :span="10">
              <div class="nameDiv">Hi,业务员,{{hoursTip}},现在是{{dateFormat(date)}}</div>
              <div class="infoDiv">欢迎来到客户关系管理系统,今天也是开心的一天哦</div>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>
    <el-row :gutter="20" style="margin-bottom: 15px">
      <el-col :span="9">
        <el-card shadow="hover"><div id="main" style="width: 100%; height: 290px"></div></el-card>
      </el-col>
      <el-col :span="9">
        <el-card shadow="hover"><div id="main2" style="width: 100%; height: 290px"></div></el-card>
      </el-col>
      <el-col :span="6">
        <el-calendar v-model="value"> </el-calendar>
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col :span="3" v-for="item in optionList" :key="item.optionId">
        <router-link :to="item.optionUrl" class="a">
          <img :src="item.optionPic" alt="" />
          <div>{{ item.optionName }}</div>
        </router-link>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import echarts from '@/lib/echarts.js'
import touxiang from '@/assets/maomi.jpg'
import customerBasicInfoPic from '@/assets/customerBasicInfo.png'
import contactsBasicInfoPic from '@/assets/contactsBasicInfo.png'
import contactInfoPic from '@/assets/contactInfo.png'
import contacterInfoPic from '@/assets/contacterInfo.png'
import actionNotesPic from '@/assets/actionNotes.png'
import actionPlanPic from '@/assets/actionPlan.png'
import adviceHandlePic from '@/assets/adviceHandle.png'
import salesgroups from '@/assets/salesgroups.png'

export default {
  name: 'MyIndex',
  data() {
    return {
      image: touxiang,
      hoursTip: '',
      value: new Date(),
      date: new Date(),
      option: {
        // 标题
        title: { text: '销售员业绩' },
        // 工具
        toolbox: {
          // 是否显示
          show: true,
          feature: {
            // 折线图 与 柱状图 切换
            magicType: {
              type: ['line', 'bar']
            },
            // 下载
            saveAsImage: {}
          }
        },
        // 节点显示
        legend: {
          data: ['战斗力']
        },
        // x轴数据
        xAxis: {
          data: ['销售员1', '销售员2', '销售员3', '销售员4', '销售员5']
        },
        // y轴
        yAxis: {},
        // 数据
        series: [
          {
            // 初始类型
            type: 'bar',
            data: [500, 400, 300, 600, 100]
          }
        ]
      },
      //
      option1 : {
        // 标题
        title: { text: '销售员业绩' },
        // 工具
        toolbox: {
          // 是否显示
          show: true,
          feature: {
            // 折线图 与 柱状图 切换
        
            // 下载
            saveAsImage: {}
          }
        },
  tooltip: {
    trigger: 'item'
  },
  legend: {
    top: '8%',
    left: 'center'
  },
  series: [
    {
      name: 'Access From',
      type: 'pie',
      radius: ['40%', '70%'],
      avoidLabelOverlap: false,
      itemStyle: {
        borderRadius: 10,
        borderColor: '#fff',
        borderWidth: 2
      },
      label: {
        show: false,
        position: 'center'
      },
      emphasis: {
        label: {
          show: true,
          fontSize: '40',
          fontWeight: 'bold'
        }
      },
      labelLine: {
        show: false
      },
      data: [
        { value: 500, name: '销售员1' },
        { value: 400, name: '销售员2' },
        { value: 300, name: '销售员3' },
        { value: 600, name: '销售员4' },
        { value: 100, name: '销售员5' }
      ]
    }
  ]
},
      // 底部选项框内容数组
      optionList: [
        { optionId: 0, optionName: '客户基础信息', optionPic: customerBasicInfoPic, optionUrl: '/home/custombasicinfo' },
        { optionId: 1, optionName: '联系人基础信息', optionPic: contactsBasicInfoPic, optionUrl: '/home/contractsbasicinfo' },
        { optionId: 2, optionName: '合约信息', optionPic: contactInfoPic, optionUrl: '/home/contractinfo' },
        { optionId: 3, optionName: '签单人信息', optionPic: contacterInfoPic, optionUrl: '/home/contracterinfo' },
        { optionId: 4, optionName: '行动记录', optionPic: actionNotesPic, optionUrl: '/home/actionnotes' },
        { optionId: 5, optionName: '行动计划', optionPic: actionPlanPic, optionUrl: '/home/actionplan' },
        { optionId: 6, optionName: '投诉建议处理', optionPic: adviceHandlePic, optionUrl: '/home/advicehandle' },
        { optionId: 7, optionName: '分组', optionPic: salesgroups, optionUrl: '/home/salesgroups' }
      ]
    }
  },
  created() {
    this.getMycount()
    this.indexPower()
  },
  methods: {
    draw() {
      // 通过echarts初始化我们的div
      let myChart = echarts.init(document.getElementById('main'))
      // 为这个echarts的图添加属性
      myChart.setOption(this.option, true)
      // 通过echarts初始化我们的div
      let myChart1 = echarts.init(document.getElementById('main2'))
      // 为这个echarts的图添加属性
      myChart1.setOption(this.option1, true)
    },
    getMycount() {
      let self = this
      let date = new Date()

      if (date.getHours() >= 0 && date.getHours() < 12) {
        self.hoursTip = '上午好'
      } else if (date.getHours() >= 12 && date.getHours() < 18) {
        self.hoursTip = '下午好'
      } else {
        self.hoursTip = '晚上好'
      }
    },
    dateFormat(time) {
          var date=new Date(time);
          var hours=date.getHours()<10 ? "0"+date.getHours() : date.getHours();
          var minutes=date.getMinutes()<10 ? "0"+date.getMinutes() : date.getMinutes();
          var seconds=date.getSeconds()<10 ? "0"+date.getSeconds() : date.getSeconds();
          // 拼接
          return hours+":"+minutes+":"+seconds;
      }


  },
  watch: {
    // 数据变化时自动重画，在控制台修改message,会自动重画
    message: function() {
      this.draw()
    }
  },
  mounted() {
    // 页面加载的时候，调用画图方法，画图
    this.draw()
     //显示当前日期时间
          let _this = this// 声明一个变量指向Vue实例this，保证作用域一致
          this.timer = setInterval(() => {
           _this.date = new Date(); // 修改数据date
           }, 1000)
  },
   beforeDestroy() {
       if (this.timer) {
        clearInterval(this.timer); // 在Vue实例销毁前，清除我们的定时器
      }
   }

}
</script>

<style lang="less" scoped>
.nameDiv {
  font-size: 20px;
  line-height: 25px;
  font-weight: bold;
  padding: 5px 0;
}
.infoDiv {
  font-size: 13px;
  color: #666;
}
.timeDiv {
  font-size: 13px;
  color: #666;
}

.char1 {
  height: 100%;
  width: 100%;
  text-align: center;
  line-height: 250px;
}
/deep/ .el-calendar-table .el-calendar-day {
  height: 30px;
}
.a {
  display: block;
  text-align: center;
  padding: 10px;
  background-color: #fff;
  border-radius: 5px;
  color: #666;
  font-family: '微软雅黑';
  img {
    width: 60px;
    height: 60px;
  }
}
.a:hover {
  background-color: #d9ecff;
  box-shadow: 0 2px 3px #ccc;
}
</style>
