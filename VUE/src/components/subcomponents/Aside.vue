<template>
  <el-aside :width="isCollapse ? '64px' : '200px'" router>
    <el-menu default-active="0" background-color="rgb(217, 236, 255)" text-color="#888" unique-opened router :collapse="isCollapse" :collapse-transition="false">
      <!-- 折叠菜单 -->
      <div class="collapse-button" @click="collapsechange">
        <span v-if="isCollapse">展开</span>
        <span v-if="!isCollapse">关闭</span>
      </div>
      <el-menu-item index="index">
        <i class="el-icon-s-home"></i>
        <span>首页</span>
      </el-menu-item>
      <!-- 一级菜单 -->
      <el-submenu :index="item.id + ''" v-for="item in tureMenuList" :key="item.id">
        <template slot="title">
          <i :class="item.icon"></i>
          <span>{{ item.authName }}</span>
        </template>
        <!-- 二级菜单 -->
        <el-menu-item :index="subitem.path + ''" v-for="subitem in item.children" :key="subitem.id">
          <i class="el-icon-menu"></i>
          <span slot="title">{{ subitem.authName }}</span>
        </el-menu-item>
      </el-submenu>
    </el-menu>
  </el-aside>
</template>

<script>
export default {
  name: 'MyAside',
  data() {
    return {
      // 管理员拥有的权限
      baseMenuList: [
        {
          id: 100,
          authName: '客户',
          icon: 'el-icon-s-custom',
          children: [
            { id: 101, authName: '客户基础信息', path: '/home/custombasicinfo' },
            { id: 102, authName: '联系人基础信息', path: '/home/contractsbasicinfo' },
            { id: 103, authName: '合约信息', path: '/home/contractinfo' },
            { id: 104, authName: '签单人信息', path: '/home/contracterinfo' }
          ]
        },
        {
          id: 110,
          authName: '客户关系',
          icon: 'el-icon-s-order',
          children: [
            { id: 111, authName: '行动记录', path: '/home/actionnotes' },
            { id: 112, authName: '行动计划', path: '/home/actionplan' },
            { id: 113, authName: '投诉建议处理', path: '/home/advicehandle' }
          ]
        },
        {
          id: 120,
          authName: '销售员',
          icon: 'el-icon-s-shop',
          children: [
            { id: 121, authName: '分组', path: '/home/salesgroups' },
            { id: 122, authName: '基础信息', path: '/home/salesbasicinfo' }
            // { id: 123, authName: '业绩', path: '/home/achievement' }
          ]
        }
        // {
        //   id: 130,
        //   authName: '用户角色',
        //   icon: 'el-icon-s-check',
        //   children: [
        //     { id: 131, authName: '角色', path: '/home/roles' },
        //     { id: 132, authName: '用户', path: '/home/users' }
        //   ]
        // }
      ],
      isCollapse: true,
      // 销售员拥有的权限
      tureMenuList: []
    }
  },
  created() {
    this.screenPower()
  },
  methods: {
    collapsechange() {
      this.isCollapse = !this.isCollapse
    },
    screenPower() {
      // 获取登录时保存的 token
      var power = window.sessionStorage.getItem('token')
      // 将 token 值进行比较，判断是否为管理员登录，并管理权限
      if (power !== 'Bearer manager') {
        // 此为销售员的权限
        this.tureMenuList = this.baseMenuList.filter(item => item.id === 100 || item.id === 110)
        return
      }
      // 此为管理员的权限
      this.tureMenuList = this.baseMenuList
    }
  }
}
</script>

<style lang="less" scoped>
.collapse-button {
  background-color: #409eff;
  font-size: 15px;
  line-height: 24px;
  color: #fff;
  text-align: center;
  letter-spacing: 0.2em;
  cursor: pointer;
}
.el-menu {
  border-right: none;
}
</style>
