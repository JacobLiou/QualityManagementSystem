<!--
 * @Author: 林伟群
 * @Date: 2022-06-14 13:57:47
 * @LastEditTime: 2022-06-14 20:50:38
 * @LastEditors: 林伟群
 * @Description: 模糊搜索名字
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\SelectUser.vue
-->
<template>
  <a-form-model-item :label="title" :prop="propType">
    <a-select
      allowClear
      show-search
      :placeholder="titlePlo"
      style="width: 100%"
      :filter-option="false"
      :not-found-content="fetching ? undefined : null"
      @search="fetchUser"
      @change="handleChange"
      v-model="value"
      option-label-prop="label"
    >
      <a-spin v-if="fetching" slot="notFoundContent" size="small" />
      <a-select-option v-for="item in data" :key="item.id" :label="item.name">
        {{ item.name }}
      </a-select-option>
    </a-select>
  </a-form-model-item>
</template>

<script>
import debounce from 'lodash/debounce'
import { getfuzzyusers } from '@/api/modular/main/SsuGroupManage'
export default {
  props: {
    title: {
      type: String,
      default: '',
    },
    propType: {
      type: String,
      default: '',
    },
  },
  data() {
    this.lastFetchId = 0
    this.fetchUser = debounce(this.fetchUser, 800)
    return {
      // 测试
      testName: '',
      data: [],
      value: '',
      fetching: false,
    }
  },
  computed: {
    titlePlo() {
      return '请输入' + this.title
    },
  },
  methods: {
    // 测试
    fetchUser(value) {
      console.log('fetching user', value)
      this.lastFetchId += 1
      const fetchId = this.lastFetchId
      this.data = []
      this.fetching = true
      getfuzzyusers({ name: value })
        .then((res) => {
          if (fetchId !== this.lastFetchId) {
            return
          }
          if (res.success) {
            this.data = res.data
            this.fetching = false
          }
        })
        .catch(() => {})
    },
    handleChange(value) {
      // this.
    },
  },
}
</script>

<style>
</style>