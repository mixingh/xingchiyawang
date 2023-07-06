<template>
  <!-- 占位符 -->
  <router-view v-if="isShow"></router-view>
</template>

<script>
export default {
  name: 'MyApp',
  provide() {
    return {
      reload: this.reload
    }
  },
  data() {
    return {
      isShow: true
    }
  },
  methods: {
    reload() {
      this.isShow = false
      this.$nextTick(() => {
        this.isShow = true
      })
    }
  }
}
</script>

<style>
</style>
