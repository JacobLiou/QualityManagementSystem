<!--
 * @Author: 林伟群
 * @Date: 2022-06-20 10:32:06
 * @LastEditTime: 2022-06-20 17:30:17
 * @LastEditors: 林伟群
 * @Description:模糊搜索名字
 * @FilePath: \frontend\src\components\SelectUser\SelectUser.vue
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
    :getPopupContainer="(triggerNode) => triggerNode.parentNode"
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
import { SsuProjectPage } from '@/api/modular/main/SsuProjectManage'
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
    queryType: {
      type: String,
      default: 'getfuzzyusers',
    },
  },
  data() {
    this.lastFetchId = 0
    this.fetchUser = debounce(this.fetchUser, 800)
    return {
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
    fetchUser(value) {
      this.lastFetchId += 1
      const fetchId = this.lastFetchId
      this.data = []
      this.fetching = true
      switch (this.queryType) {
        case 'SsuProjectPage':
          SsuProjectPage({ ProjectName: value })
            .then((res) => {
              if (fetchId !== this.lastFetchId) {
                return
              }
              if (res.success) {
                this.data = res.data.rows?.map((item) => {
                  const data = {
                    id: item.id,
                    name: item.projectName,
                  }
                  return data
                })

                this.fetching = false
              }
            })
            .catch(() => {})
          break
        default:
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
          break
      }
    },
    handleChange(value, option) {
      const label = option?.componentOptions?.propsData?.label
      this.$emit('handlerSelectUser', { value, label, selectType: this.selectType })
    },
  },
}
</script>

<style>
</style>