<!--
 * @Author: 林伟群
 * @Date: 2022-05-18 11:07:28
 * @LastEditTime: 2022-05-18 11:12:47
 * @LastEditors: 林伟群
 * @Description: 历史记录组件
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\OperRecords.vue
-->
<template>
  <section class="info">
    <div class="title">历史记录</div>
    <a-row :gutter="[24, 12]">
      <a-col :xl="12" :xs="24"
        ><ul v-if="this.operationRecords != ''">
          <li v-for="(item, index) in operationRecords" :key="index" :value="item.operationTypeId">
            {{ index + 1 }}、{{ item.operationTime }}, 由 <b>{{ item.operatorName }}</b>
            {{ 'issue_operation_type' | dictType(item.operationTypeId) }}
          </li>
        </ul>
      </a-col>
      <a-col :xl="12" :xs="24">
        <a-steps direction="vertical" class="step">
          <a-step
            v-for="(item, index) in operationRecords"
            :key="index"
            status="process"
            :title="'issue_operation_type' | dictType(item.operationTypeId)"
            :description="item.operationTime"
          />
        </a-steps>
      </a-col>
    </a-row>
  </section>
</template>

<script>
import { IssueOperationPage } from '@/api/modular/main/SsuIssueManage'
export default {
  data() {
    return {
      record: {},
      operationRecords: [], // 操作记录
    }
  },
  created() {
    // checkRecord选中的要实时更新
    this.record = this.$store.state.record.checkRecord
    this.getIssueOperationPage()
  },
  methods: {
    // 获取历史记录
    getIssueOperationPage() {
      this.record.issueId = this.record.id
      IssueOperationPage(this.record)
        .then((res) => {
          if (res.success) {
            const operationRecords = res.data.rows
            this.operationRecords = operationRecords.reverse()
          } else {
            this.$message.error('问题操作记录读取失败')
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },
  },
}
</script>
<style lang="less" scoped>
.info {
  width: 100%;
  margin-bottom: 1.5em;
  .title {
    width: 80px;
    flex-shrink: 0;
    font-size: 1.1em;
    font-weight: 700;
  }
  ul {
    margin-bottom: 2em;
    li {
      margin: 2em 0;
      margin-left: -1em;
      list-style: none;
    }
  }
  .step {
    margin-left: 16px;
  }
}
</style>