<!--
 * @Author: 林伟群
 * @Date: 2022-06-14 13:57:47
 * @LastEditTime: 2022-06-15 15:49:50
 * @LastEditors: 林伟群
 * @Description: 模糊搜索名字
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\SelectUser.vue
-->
<template>
  <a-select
    allowClear
    show-search
    :placeholder="title"
    style="width: 100%"
    :filter-option="false"
    :not-found-content="fetching ? undefined : null"
    @search="fetchUser"
    @change="handleChange"
    @deselect="handleChange"
    v-model="value"
    option-label-prop="label"
  >
    <a-spin v-if="fetching" slot="notFoundContent" size="small" />
    <a-select-option v-for="item in data" :key="item.id" :label="item.name">
      {{ item.name }}
    </a-select-option>
  </a-select>
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
    userSelect: {
      type: Object,
    },
    selectType: {
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
  watch: {
    userSelect: {
      handler() {
        this.data = []
        this.data.push(this.userSelect)
        this.value = this.userSelect.id
      },
      deep: true,
      immediate: true,
    },
  },
  methods: {
    // 测试
    fetchUser(value) {
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
    handleChange(value, option) {
      const label = option?.componentOptions?.propsData?.label
      console.log('selectType', this.selectType)
      this.$emit('handlerSelectUser', { value, label, selectType: this.selectType })
    },
  },
}
</script>

<style>
</style>