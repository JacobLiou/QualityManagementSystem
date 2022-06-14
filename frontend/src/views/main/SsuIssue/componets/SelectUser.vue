<!--
 * @Author: 林伟群
 * @Date: 2022-06-14 13:57:47
 * @LastEditTime: 2022-06-14 14:56:23
 * @LastEditors: 林伟群
 * @Description: 模糊搜索名字
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\SelectUser.vue
-->
<template>
  <a-form-model-item :label="title">
    <section class="from_chilen">
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
      >
        <a-spin v-if="fetching" slot="notFoundContent" size="small" />
        <a-select-option v-for="(item, index) in data" :key="index" :value="item.id">{{ item.name }}</a-select-option>
      </a-select>
    </section>
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
  },
  data() {
    this.lastFetchId = 0
    this.fetchUser = debounce(this.fetchUser, 800)
    return {
      // 测试
      testName: '',
      data: [],
      value: [],
      fetching: false,
    }
  },
  computed: {
    titlePlo() {
      return '请输入' + this.title
    },
  },
  watch: {
    value() {
      console.log(this.value)
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
          if (res.success) {
            this.data = res.data
            this.fetching = false
          }
        })
        .catch(() => {})
    },
    handleChange(value) {
      Object.assign(this, {
        value,
        data: [],
        fetching: false,
      })
    },
  },
}
</script>

<style>
</style>