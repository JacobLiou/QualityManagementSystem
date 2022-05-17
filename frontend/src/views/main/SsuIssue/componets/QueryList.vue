<!--
 * @Author: 林伟群
 * @Date: 2022-05-11 15:54:20
 * @LastEditTime: 2022-05-17 11:44:26
 * @LastEditors: 林伟群
 * @Description: 高级筛选
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\QueryList.vue
-->
<template>
  <a-modal v-model="visible" title="高级查询" on-ok="handleOk">
    <a-row :gutter="[12, 12]" align="middle" type="flex">
      <a-col :xs="8">
        <a-select v-model="CreatorType" style="width: 100%" placeholder="提出人">
          <a-select-option v-for="(item, index) in CreatorList" :key="index" :value="item.value">{{
            item.name
          }}</a-select-option>
        </a-select>
      </a-col>
      <a-col :xs="16">
        <a-input v-model="nameList" allow-clear placeholder="请输入名字" />
      </a-col>
    </a-row>
    <a-row :gutter="[12, 12]" align="middle" type="flex">
      <a-col :xs="8">
        <a-select v-model="CreatorType" style="width: 100%" placeholder="时间类型">
          <a-select-option v-for="(item, index) in createTimeNameList" :key="index" :value="item.value">{{
            item.timeName
          }}</a-select-option>
        </a-select>
      </a-col>
      <a-col :xs="16">
        <a-date-picker
          style="width: 100%"
          :placeholder="placeholderTime.start"
          @change="stateTime"
          v-model="stateDate"
        />
      </a-col>
      <a-col :push="8" :xs="16">
        <a-date-picker style="width: 100%" :placeholder="placeholderTime.end" @change="endTime" v-model="endDate" />
      </a-col>
    </a-row>
    <a-row :gutter="[12, 12]" align="middle" type="flex">
      <a-col :xs="8"> <div class="title_style">问题分类</div> </a-col>
      <a-col :xs="16">
        <a-select :allowClear="true" v-model="queryParam.IssueClassification" style="width: 100%" placeholder="请选择问题分类">
          <a-select-option v-for="(item, index) in classificationList" :key="index" :value="item.code">{{
            item.name
          }}</a-select-option>
        </a-select>
      </a-col>
    </a-row>
    <a-row :gutter="[12, 12]" align="middle" type="flex">
      <a-col :xs="8"> <div class="title_style">工序</div> </a-col>
      <a-col :xs="16">
        <a-select :allowClear="true" v-model="queryParam.Process" style="width: 100%" placeholder="请选择工序">
          <a-select-option v-for="(item, index) in processList" :key="index" :value="item.code">{{
            item.name
          }}</a-select-option>
        </a-select>
      </a-col>
    </a-row>
    <a-row :gutter="[12, 12]" align="middle" type="flex">
      <a-col :xs="8"> <div class="title_style">测试类别</div> </a-col>
      <a-col :xs="16">
        <a-select :allowClear="true" v-model="queryParam.TestClassification" style="width: 100%" placeholder="请选择测试类别">
          <a-select-option v-for="(item, index) in testClassificationList" :key="index" :value="item.code">{{
            item.name
          }}</a-select-option>
        </a-select>
      </a-col>
    </a-row>
    <template slot="footer">
      <a-row :gutter="[12, 12]" align="middle" justify="center" type="flex">
        <a-col :xs="6">
          <a-button @click="handleQuery" type="primary"> 检索 </a-button>
        </a-col>
        <a-col :xs="6">
          <a-button @click="handleReset" type="primary"> 重置 </a-button>
        </a-col>
        <a-col :xs="6">
          <a-button @click="handleCancel"> 返回 </a-button>
        </a-col>
      </a-row>
    </template>
  </a-modal>
</template>

<script>
export default {
  props: {
    visibleTrue: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      visible: false,
      queryParam: {},
      nameList: '',
      CreatorList: [
        {
          name: '提出人',
          value: 'Creator',
        },
        {
          name: '转发人',
          value: 'Dispatcher',
        },
        {
          name: '解决人',
          value: 'Executor',
        },
      ],
      CreatorType: 'Creator',
      createTimeNameList: [
        {
          timeName: '提出时间',
          value: 'Creator',
        },
        {
          timeName: '转发时间',
          value: 'Dispatcher',
        },
        {
          timeName: '解决时间',
          value: 'Executor',
        },
      ],
      classificationList: [], // 问题分类列表
      processList: [], // 工序列表
      testClassificationList: [], // 测试列表
      stateDate: '',
      endDate: '',
    }
  },
  watch: {
    visibleTrue() {
      this.visible = !this.visible
    },
  },
  computed: {
    placeholderTime() {
      this.stateDate = ''
      this.endDate = ''
      let content = {
        start: '请选择提出时间起点',
        end: '请选择提出时间终点',
      }
      switch (this.CreatorType) {
        case 'Creator':
          content = {
            start: '请选择提出时间起点',
            end: '请选择提出时间终点',
          }
          break
        case 'Dispatcher':
          content = {
            start: '请选择转发时间起点',
            end: '请选择转发时间终点',
          }
          break
        case 'Executor':
          content = {
            start: '请选择解决时间起点',
            end: '请选择解决时间终点',
          }
          break
        default:
          break
      }
      return content
    },
  },
  created() {
    this.classificationList = this.$options.filters['dictData']('issue_classification')
    this.processList = this.$options.filters['dictData']('trail_production_process')
    this.testClassificationList = this.$options.filters['dictData']('test_classification')
  },
  methods: {
    handleOk() {
      this.visible = false
    },
    // 高级检索
    handleQuery() {
      if (this.nameList !== '') {
        switch (this.CreatorType) {
          case 'Creator':
            this.queryParam.Creator = this.nameList
            break
          case 'Dispatcher':
            this.queryParam.Dispatcher = this.nameList
            break
          case 'Executor':
            this.queryParam.Executor = this.nameList
            break
          default:
            break
        }
      }
      this.$emit('queryLsit', this.queryParam)
      this.visible = false
    },

    // 重置
    handleReset() {
      this.nameList = ''
      this.queryParam = {}
      this.stateDate = ''
      this.endDate = ''
      this.CreatorType = 'Creator'
    },
    // 取消
    handleCancel() {
      this.visible = false
    },

    stateTime(date, dateString) {
      switch (this.CreatorType) {
        case 'Creator':
          this.queryParam.CreateTimeFrom = dateString
          break
        case 'Dispatcher':
          this.queryParam.DispatchTimeFrom = dateString
          break
        case 'Executor':
          this.queryParam.SolveTimeFrom = dateString
          break
        default:
          break
      }
    },
    endTime(date, dateString) {
      switch (this.CreatorType) {
        case 'Creator':
          this.queryParam.CreateTimeTo = dateString
          break
        case 'Dispatcher':
          this.queryParam.DispatchTimeTo = dateString
          break
        case 'Executor':
          this.queryParam.SolveTimeTo = dateString
          break
        default:
          break
      }
    },
  },
}
</script>

<style lang="less" scoped>
.title_style {
  width: 100%;
  text-align: right;
}
</style>