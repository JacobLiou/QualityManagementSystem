<!--
 * @Author: 林伟群
 * @Date: 2022-05-11 15:54:20
 * @LastEditTime: 2022-06-23 10:16:43
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
        <section class="check_btn">
          <SelectUser title="请输入名字" @handlerSelectUser="handlerSelectUser" :userSelect="userSelect"></SelectUser>
          <a-button @click="changePersonnel(CreatorType)"> 选择 </a-button>
        </section>
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
        <a-select
          :allowClear="true"
          v-model="queryParam.IssueClassification"
          style="width: 100%"
          placeholder="请选择问题分类"
        >
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
        <a-select
          :allowClear="true"
          v-model="queryParam.TestClassification"
          style="width: 100%"
          placeholder="请选择测试类别"
        >
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
import SelectUser from './SelectUser.vue'
export default {
  components: {
    SelectUser,
  },
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
      nameId: null,
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
    userSelect() {
      return {
        id: this.nameId,
        name: this.nameList,
      }
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

    // 模糊搜索选中人员
    handlerSelectUser(valueObj) {
      this.nameId = valueObj.value
      this.nameList = valueObj.label
    },
    // 高级检索
    handleQuery() {
      if (this.nameList !== '') {
        this.queryParam.Creator = undefined
        this.queryParam.Dispatcher = undefined
        this.queryParam.Executor = undefined
        switch (this.CreatorType) {
          case 'Creator':
            this.queryParam.Creator = this.nameId
            break
          case 'Dispatcher':
            this.queryParam.Dispatcher = this.nameId
            break
          case 'Executor':
            this.queryParam.Executor = this.nameId
            break
          default:
            break
        }
      }
      console.log(this.queryParam)
      this.$emit('queryLsit', this.queryParam)
      this.visible = false
    },

    changePersonnel(value) {
      this.$parent.userVisible = !this.$parent.userVisible
      this.$emit('changePersonnel', value)
    },

    // 重置
    handleReset() {
      this.nameList = ''
      this.stateDate = ''
      this.endDate = ''
      this.CreatorType = 'Creator'
      for (const key in this.queryParam) {
        this.queryParam[key] = undefined
      }
    },
    // 取消
    handleCancel() {
      this.visible = false
    },

    stateTime(date, dateString) {
      this.queryParam.CreateTimeFrom = undefined
      this.queryParam.DispatchTimeFrom = undefined
      this.queryParam.SolveTimeFrom = undefined
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
      this.queryParam.CreateTimeTo = undefined
      this.queryParam.DispatchTimeTo = undefined
      this.queryParam.SolveTimeTo = undefined
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
.check_btn {
  display: flex;
}
</style>