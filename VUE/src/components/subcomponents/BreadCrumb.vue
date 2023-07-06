<template>
  <div>
    <!-- 面包屑区域 -->
    <el-breadcrumb separator="/">
      <el-breadcrumb-item :to="{ path: '/home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>{{ firstname }}</el-breadcrumb-item>
      <el-breadcrumb-item>{{ secondname }}</el-breadcrumb-item>
    </el-breadcrumb>
  </div>
</template>

<script>
export default {
  name: 'MyBreadCrumb',
  props: ['firstname', 'secondname']
}
</script>

<style lang="less" scoped>
</style>
