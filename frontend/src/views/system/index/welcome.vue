<template>
  <a-card :bordered="false" style="display: flex;justify-content:left;height: 100%" >
    <div style="margin:100px auto;">
       <h2>快捷导航</h2>
       <router-link class="problemTable" :to="{ name: 'problemTable' }">问题管理</router-link>
    </div>
  </a-card>
</template>

<script>

</script>

<style lang="less" scoped>

</style>
