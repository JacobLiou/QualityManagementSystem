<!--
 * @Author: 林伟群
 * @Date: 2022-06-14 13:57:47
 * @LastEditTime: 2022-06-15 16:53:30
 * @LastEditors: 林伟群
 * @Description: 模糊搜索名字多选
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\SelectUserMore.vue
-->
<template>
  <a-select
    mode="multiple"
    label-in-value
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
      type: Array,
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
      data: [],
      value: [],
      fetching: false,
    }
  },
  watch: {
    userSelect: {
      handler() {
        this.data = this.userSelect
        this.value = this.userSelect.map((item) => {
          return { key: item.id, label: item.name }
        })
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
    handleChange() {
      console.log(this.value)
      this.$emit('handlerSelectUser', { value: this.value, selectType: this.selectType })
    },
  },
}
</script>

<style>
</style>