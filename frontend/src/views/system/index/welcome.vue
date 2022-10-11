<!--
 * @Author: 林伟群
 * @Date: 2022-06-10 09:25:01
 * @LastEditTime: 2022-06-21 20:06:45
 * @LastEditors: 林伟群
 * @Description: 请输入文件功能类型
 * @FilePath: \frontend\src\views\system\index\welcome.vue
-->
<template>
  

 
  <a-card class="issuelink" :bordered="false" @click="handlePushPath('/ssuissue')" ></a-card>


</template>

<script>
export default {
  created() {
    const toPath = sessionStorage.getItem('to_path_to')
    if (toPath) {
      this.$router.replace({ path: toPath })
      sessionStorage.getItem('to_path_to', '')
    }
  },
  methods: {
    handlePushPath(path) {
      this.$router.push({ path })
    },
  },
}
</script>

<style lang="less" scoped>

.issuelink {
  width: 80%;
  height: 80%;
  background-image: url('../../../assets/technologicalProcess.png');
  background-repeat: no-repeat;
  background-size: 100% 100%;
  position: relative;
  padding: 60px;
  cursor: pointer;
}
</style>
