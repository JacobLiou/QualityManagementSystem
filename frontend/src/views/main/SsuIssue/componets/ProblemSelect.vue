<!--
 * @Author: 林伟群
 * @Date: 2022-05-11 10:36:55
 * @LastEditTime: 2022-05-17 11:41:09
 * @LastEditors: 林伟群
 * @Description: 问题管理筛选组件
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ProblemSelect.vue
-->
<template>
  <section>
    <a-card>
      <a-row :gutter="[36, 12]" align="middle" type="flex">
        <a-col :xxl="3" :xl="4" :md="6" :xs="12">
          <a-select :allowClear="true" v-model="queryParam.projectId" style="width: 100%" placeholder="项目编号">
            <a-select-option v-for="(item, index) in projectData" :key="index" :value="item.id">{{
              item.projectName
            }}</a-select-option>
          </a-select>
        </a-col>
        <a-col :xxl="2" :xl="3" :md="6" :xs="12">
          <a-select :allowClear="true" v-model="queryParam.module" style="width: 100%" placeholder="模块">
            <a-select-option v-for="(item, index) in moduleData" :key="index" :value="item.code">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-col>
        <a-col :xxl="2" :xl="3" :md="6" :xs="12">
          <a-select :allowClear="true" style="width: 100%" v-model="queryParam.consequence" placeholder="性质">
            <a-select-option v-for="(item, index) in consequenceData" :key="index" :value="item.code">{{
              item.name
            }}</a-select-option>
          </a-select></a-col
        >
        <a-col :xxl="2" :xl="3" :md="6" :xs="12">
          <a-select :allowClear="true" style="width: 100%" v-model="queryParam.status" placeholder="状态">
            <a-select-option v-for="(item, index) in statusData" :key="index" :value="item.code">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-col>
        <a-col :xxl="5" :xl="6" :md="12" :xs="24">
          <a-input v-model="queryParam.title" allow-clear placeholder="请输入问题关键词" />
        </a-col>
        <a-col :xxl="2" :xl="3" :md="6" :xs="12">
          <a-button style="width: 100%" type="primary" @click="handleQueryParam">搜索</a-button>
        </a-col>
        <a-col :xxl="1" :xl="2" :md="4" :xs="6"
          ><a-icon @click="openQuerylist" class="problem_icon" :style="{ fontSize: '2em' }" type="filter"
        /></a-col>
      </a-row>
    </a-card>
    <QueryList :visibleTrue="visibleTrue" @queryLsit="queryLsit"></QueryList>
  </section>
</template>

<script>
import { SsuProjectList } from '@/api/modular/main/SsuProjectManage'
import QueryList from './QueryList.vue'
export default {
  components: {
    QueryList,
  },
  data() {
    return {
      queryParam: {},
      projectData: [], // 项目列表数据
      moduleData: [],
      consequenceData: [],
      statusData: [],
      visibleTrue: false, // 弹窗
    }
  },
  created() {
    this.getProjectList()
    const moduleOption = this.$options
    this.moduleData = moduleOption.filters['dictData']('issue_module')
    const consequenceOption = this.$options
    this.consequenceData = consequenceOption.filters['dictData']('issue_consequence')
    const statusOption = this.$options
    this.statusData = statusOption.filters['dictData']('issue_status')
  },
  methods: {
    // 获取项目列表
    getProjectList() {
      SsuProjectList()
        .then((res) => {
          if (res.success) {
            this.projectData = res.data
          } else {
            this.$message.error('项目列表读取失败')
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },

    // 打开筛选弹窗
    openQuerylist() {
      this.visibleTrue = !this.visibleTrue
    },

    queryLsit(val) {
      console.log(val);
      this.$emit('queryParamSelect', val)
    },

    handleQueryParam() {
      this.$emit('queryParamSelect', this.queryParam)
      // 问题搜索
      //   this.$refs.table.refresh(true) // 原有搜索页面
    },
  },
}
</script>

<style lang="less" scoped>
.problem_icon {
  cursor: pointer;
}
</style>