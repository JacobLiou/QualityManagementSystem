<!--
 * @Author: 林伟群
 * @Date: 2022-05-20 17:57:57
 * @LastEditTime: 2022-06-21 15:06:26
 * @LastEditors: 林伟群
 * @Description: 属性选择组件
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\AttributCheck.vue
-->
<template>
  <a-modal v-model="visible" title="请选择属性" on-ok="handleOk" destroyOnClose forceRender>
    <section v-if="attributArray.length == 0">暂无可选择属性</section>
    <section v-else>
      <!-- <a-checkbox-group :options="attributArray" :default-value="initCheckAttr" @change="checkChange" /> -->
      <a-checkbox-group @change="checkChange" :defaultValue="checkedValues">
        <a-row :gutter="[12, 12]">
          <a-col :span="12" v-for="(item, index) in attributArray" :key="index">
            <a-checkbox :value="item.value" :disabled="item.label == '问题性质评分'"> {{ item.label }} </a-checkbox>
          </a-col>
        </a-row>
      </a-checkbox-group>
    </section>
    <template slot="footer">
      <a-row :gutter="[12, 12]" align="middle" justify="center" type="flex">
        <a-col :xs="6">
          <a-button @click="handleAttribut" type="primary"> 保存 </a-button>
        </a-col>
        <a-col :xs="6">
          <a-button @click="handleCancel">关闭 </a-button>
        </a-col>
      </a-row>
    </template>
  </a-modal>
</template>

<script>
import { IssueExtAttrListStruct } from '@/api/modular/main/SsuIssueExtendAttributeManage'
export default {
  props: ['attributVisible', 'moduleType', 'initCheckAttr'],
  data() {
    return {
      visible: false,
      attributArray: [],
      checkedValues: [], // 选中的数据
    }
  },
  watch: {
    attributVisible() {
      this.visible = !this.visible
    },
    moduleType: {
      handler() {
        if (this.moduleType == undefined) return
        this.checkedValues = []
        const parameter = { Module: Number(this.moduleType) }
        IssueExtAttrListStruct(parameter)
          .then((res) => {
            if (res.success) {
              this.attributArray = res.data.map((item) => {
                const itemCheck = {
                  label: item.fieldName,
                  value: JSON.stringify(item),
                }
                return itemCheck
              })
              // 筛选出存在的属性
              this.checkedValues = this.initCheckAttr.filter((item) => {
                const itemIndex = this.attributArray.findIndex((it) => {
                  if (it.value == item) return it
                })
                if (itemIndex > 0) return item
              })
              // 当其为试产模式时，问题性质得分自动生成，跟随性质自动评分，不可操作
              if (this.moduleType == 2) {
                const getTrial = this.attributArray.find((item) => {
                  return item.label == '问题性质评分'
                })
                this.checkedValues = [...new Set([...this.checkedValues, getTrial.value])]
                this.$emit('handleAttribut', this.checkedValues)
              }
            } else {
              this.attributArray = []
              this.$message.warning('模块属性获取失败')
            }
          })
          .catch(() => {
            this.attributArray = []
            this.$message.warning('模块属性获取失败')
          })
      },
      immediate: true,
    },
  },
  methods: {
    // 选中的属性
    checkChange(val) {
      this.checkedValues = val
      // console.log('默认选中', this.checkedValues)
    },
    handleOk() {
      this.visible = false
    },
    handleAttribut() {
      this.$emit('handleAttribut', this.checkedValues)
      this.visible = false
    },
    handleCancel() {
      this.visible = false
    },
  },
}
</script>

<style>
</style>