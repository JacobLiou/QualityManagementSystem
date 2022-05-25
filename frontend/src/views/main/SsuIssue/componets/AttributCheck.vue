<!--
 * @Author: 林伟群
 * @Date: 2022-05-20 17:57:57
 * @LastEditTime: 2022-05-23 15:58:52
 * @LastEditors: 林伟群
 * @Description: 属性选择组件
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\AttributCheck.vue
-->
<template>
  <a-modal v-model="visible" title="请选择属性" on-ok="handleOk" destroyOnClose forceRender>
    <section v-if="attributArray.length == 0">暂无可选择属性</section>
    <section v-else>
      <!-- <a-checkbox-group :options="attributArray" :default-value="checkedValues" @change="checkChange" /> -->
      <a-checkbox-group @change="checkChange">
        <a-row :gutter="[12, 12]">
          <a-col :span="12" v-for="(item, index) in attributArray" :key="index">
            <a-checkbox :value="item.value"> {{ item.label }} </a-checkbox>
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
  props: ['attributVisible', 'moduleType'],
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