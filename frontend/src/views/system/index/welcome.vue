<!--
 * @Author: 林伟群
 * @Date: 2022-06-10 09:25:01
 * @LastEditTime: 2022-06-14 10:01:48
 * @LastEditors: 林伟群
 * @Description: 请输入文件功能类型
 * @FilePath: \frontend\src\views\system\index\welcome.vue
-->
<template>
  <a-card :bordered="false" style="display: flex; justify-content: left; height: 100%">
    <div style="margin: 100px auto">
      <h2>快捷导航</h2>
      <router-link class="problemTable" :to="{ name: 'problemTable' }">问题管理</router-link>
    </div>
  </a-card>
</template>

<script>
export default {
  created() {
    const toPath = sessionStorage.getItem('to_path_to')
    if (toPath) {
      this.$store.commit('SET_CHECK_PATH', true) // 路径原路返回
      this.$router.replace({ path: toPath })
      sessionStorage.getItem('to_path_to', '')
    }
  },
}
</script>

<style lang="less" scoped>
</style>
