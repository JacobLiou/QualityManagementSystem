<!--
 * @Author: 林伟群
 * @Date: 2022-05-12 20:57:21
 * @LastEditTime: 2022-05-17 17:19:33
 * @LastEditors: 林伟群
 * @Description: 表格
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\Table.vue
-->
<template>
  <section>
    <a-table
      :columns="columns"
      :row-key="
        (record, index) => {
          return index
        }
      "
      :data-source="issueData"
      :pagination="false"
      @change="handleTableChange"
      :scroll="{ x: true, y: moblileTrue ? 0 : 'calc(100vh - 446px)' }"
    >
      <div slot="checkbox" slot-scope="text, record, index">
        <a-checkbox @change="onceCheck(text, record, index)" :checked="record.checkbox"> </a-checkbox>
      </div>
      <!-- 操作 -->
      <section slot="operation" class="table_operation" slot-scope="text, record">
        <a-dropdown :trigger="['click']" :getPopupContainer="(triggerNode) => triggerNode.parentNode">
          <a class="ant-dropdown-link" @click="(e) => e.preventDefault()"> 更多 <a-icon type="down" /> </a>
          <a-menu slot="overlay">
            <a-menu-item
              :key="item.currentKey"
              v-for="item in operationFilter(record.status)"
              @click="operationType(record, item.operName)"
            >
              <a-icon :style="{ fontSize: '1em' }" :type="item.operIcon" />{{ item.operName }}
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </section>
    </a-table>
    <section class="record_footer">
      <div class="footer_left">
        <a-checkbox class="left_check" :indeterminate="indeterminate" :checked="checkAll" @change="CheckAllChange">
          全选
        </a-checkbox>
      </div>
      <div class="footer_center" v-if="checkShow">
        <a-button @click="checkForward" class="center">转交</a-button>
        <a-button @click="checkExport" class="center">导出</a-button>
      </div>
      <a-pagination
        class="content_pagination"
        :total="totalNum"
        :current="queryParam.PageNo"
        :pageSize="queryParam.PageSize"
        :pageSizeOptions="pageSizeOptions"
        show-size-changer
        show-quick-jumper
        @change="jumpPagination"
        @showSizeChange="changePageSize"
      />
    </section>
    <!-- 问题挂起弹窗 -->
    <HangProblem ref="hangProblem"></HangProblem>
  </section>
</template>

<script>
import { IssueDelete, IssueExport, Downloadfile } from '@/api/modular/main/SsuIssueManage'
import HangProblem from './HangProblem.vue'
export default {
  components: {
    HangProblem,
  },
  props: {
    columns: {
      type: Array,
      default() {
        return []
      },
    },
    issueList: {
      type: Array,
      default() {
        return []
      },
    },
    queryParam: {
      type: Object,
    },
    totalNum: {
      type: Number,
    },
  },
  data() {
    return {
      issueData: [],
      indeterminate: false,
      checkAll: false,
      pageSizeOptions: ['20', '30', '40', '50'],
      operationList: [
        {
          operName: '分发',
          operIcon: 'select',
        },
        {
          operName: '解决',
          operIcon: 'question-circle',
        },
        {
          operName: '验证',
          operIcon: 'safety-certificate',
        },
        {
          operName: '转交',
          operIcon: 'export',
        },
        {
          operName: '挂起',
          operIcon: 'minus-circle',
        },
        {
          operName: '详情',
          operIcon: 'file-done',
        },
        {
          operName: '编辑',
          operIcon: 'edit',
        },
        {
          operName: '删除',
          operIcon: 'delete',
        },
      ],
    }
  },
  watch: {
    issueList: {
      handler() {
        if (this.issueList.length === 0) this.issueData = []
        const newIssueList = this.issueList.map((item) => {
          item.checkbox = false
          return item
        })
        this.issueData = newIssueList
      },
      deep: true,
      immediate: true,
    },
  },

  computed: {
    checkShow() {
      return this.indeterminate ? true : this.checkAll
    },
  },

  created() {
    this.isMoblile()
  },
  methods: {
    operationFilter(state) {
      let operationList = [
        {
          operName: '复制',
          operIcon: 'copy',
        },
        {
          operName: '详情',
          operIcon: 'file-done',
        },
        {
          operName: '编辑',
          operIcon: 'edit',
        },
        {
          operName: '删除',
          operIcon: 'delete',
        },
      ]
      const operationAdd = {
        0: [
          {
            operName: '分发',
            operIcon: 'select',
          },
        ],
        1: [
          {
            operName: '解决',
            operIcon: 'question-circle',
          },

          {
            operName: '转交',
            operIcon: 'export',
          },
          {
            operName: '挂起',
            operIcon: 'minus-circle',
          },
        ],
        2: [
          {
            operName: '验证',
            operIcon: 'safety-certificate',
          },
          {
            operName: '转交',
            operIcon: 'export',
          },
        ],
        3: [
          {
            operName: '转交',
            operIcon: 'export',
          },
          {
            operName: '挂起',
            operIcon: 'minus-circle',
          },
        ],
        4: [],
        5: [
          {
            operName: '分发',
            operIcon: 'select',
          },
        ],
      }
      const addList = operationAdd[String(state)]
      const newOperationList = [...addList, ...operationList]
      return newOperationList
    },
    isMoblile() {
      if (
        window.navigator.userAgent.match(
          /(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone)/i
        )
      ) {
        this.moblileTrue = true
      } else {
        this.moblileTrue = false
      }
    },
    // 降序排序
    handleTableChange(text, record, index) {
      const { field, order } = index
      this.$emit('handleTableChange', { SortField: field, SortOrder: order })
    },

    // 选中
    onceCheck(text, record, index) {
      const newRecord = record
      newRecord.checkbox = !text
      this.issueData.splice(index, 1, newRecord)
      const newIssueData = this.issueData.filter((item) => {
        return item.checkbox == true
      })
      this.indeterminate = !!newIssueData.length && newIssueData.length < this.issueData.length
      this.checkAll = newIssueData.length === this.issueData.length
    },
    // 全选
    CheckAllChange() {
      if (this.checkAll) {
        this.checkAll = false
        this.indeterminate = false
        this.issueData.forEach((item) => {
          item.checkbox = false
        })
      } else {
        this.checkAll = true
        this.indeterminate = false
        this.issueData.forEach((item) => {
          item.checkbox = true
        })
      }
    },

    // 操作类型
    operationType(record, operName) {
      // console.log(record, operName)
      switch (operName) {
        case '删除':
          this.problemDelect(record)
          break
        case '挂起':
          this.$refs.hangProblem.edit(record)
          break
        case '详情':
          this.$router.push({
            path: '/problemInfo',
            query: { id: record.id },
          })
          this.$store.commit('SET_CHECK_RECORD', record)
          break
        default:
          break
      }
    },

    // 删除
    problemDelect(record) {
      const { id } = record
      const _this = this
      console.log(this.$parent.$parent.$parent.getProblemList())
      this.$confirm({
        content: '确定删除该问题',
        onOk() {
          IssueDelete({ id }).then((res) => {
            if (res.success) {
              _this.$message.success('删除成功')
              _this.$parent.$parent.$parent.getProblemList()
            } else {
              _this.$message.error('删除失败')
            }
          })
        },
        onCancel() {},
      })
    },

    // 选中转发
    checkForward() {
      // TODO
    },

    // 选中导出
    checkExport() {
      // TODO
      console.log(this.issueData)
      const exportData = this.issueData.filter((item) => item.checkbox)
      console.log(exportData)
      const exportPamter = exportData.map((item) => {
        return item.id
      })
      IssueExport(exportPamter)
        .then((res) => {
          this.confirmLoading = false
          Downloadfile(res)
        })
        .catch((err) => {
          this.confirmLoading = false
          this.$message.error('下载错误：获取文件流错误' + err)
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },

    jumpPagination(PageNo, PageSize) {
      this.$emit('queryProblem', { PageNo, PageSize })
    },
    changePageSize(PageNo, PageSize) {
      this.$emit('queryProblem', { PageNo, PageSize })
    },
  },
}
</script>

<style lang="less" scoped>
.record_footer {
  margin-top: 1.5em;
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
  vertical-align: middle;
  .footer_left {
    margin-bottom: 1.5em;
    .left_check {
      padding: 0 16px;
    }
  }
  .footer_center {
    margin-bottom: 1.5em;
    .center {
      margin-right: 1em;
    }
  }
}

/deep/.ant-table-body {
  min-width: 200px !important;
}
.table_operation {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  max-width: 100px;
  .operation_once {
    cursor: pointer;
    width: 50%;
    margin-bottom: 12px;
    .once_span {
      text-align: center;
    }
  }
}
</style>