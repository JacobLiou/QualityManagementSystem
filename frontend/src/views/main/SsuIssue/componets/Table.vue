<!--issueData
 * @Author: 林伟群
 * @Date: 2022-05-12 20:57:21
 * @LastEditTime: 2022-05-31 20:33:13
 * @LastEditors: 林伟群
 * @Description: 表格   :scroll="{ x: true, y: moblileTrue ? 0 : 'calc(100vh - 360px)' }"
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\Table.vue
-->
<template>
  <section :class="{ problem_table: isProblemTable }">
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
      :scroll="{ x: true, y: moblileTrue ? 0 : 'calc(100vh - 360px)' }"
      size="middle"
      :customRow="customRow"
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
              v-for="item in operationFilter(record.btnList)"
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
    <!-- 问题解决弹窗 -->
    <ProblemSolve ref="problemSolve"></ProblemSolve>
    <!-- 问题复核 -->
    <ProblemRecheck ref="recheckProblem"></ProblemRecheck>
    <!-- 问题验证 -->
    <ProblemValidate ref="validateProblem"></ProblemValidate>
    <!-- 问题转交 -->
    <ProblemRedispatch ref="redispatchProblem" @changePersonnel="changePersonnel"></ProblemRedispatch>
    <!-- 问题挂起 -->
    <ProblemHang ref="hangProblem"></ProblemHang>
    <!-- 问题关闭 -->
    <ProblemClose ref="closeProblem"></ProblemClose>
    <!-- 批量转交 -->
    <ProblemBatchRedispatch ref="batchRedispatchProblem" @changePersonnel="changePersonnel"></ProblemBatchRedispatch>
    <!-- 选择人员 -->
    <CheckUserList
      class="checkUser"
      :userVisible="userVisible"
      :personnelType="personnelType"
      @checkUserArray="checkUserArray"
    ></CheckUserList>
  </section>
</template>

<script>
import { IssueDelete, IssueExport, Downloadfile, IssueReOpen, IssueSendur } from '@/api/modular/main/SsuIssueManage'
import ProblemSolve from './ProblemSolve.vue'
import ProblemRecheck from './ProblemRecheck.vue'
import ProblemValidate from './ProblemValidate.vue'
import ProblemRedispatch from './ProblemRedispatch.vue'
import ProblemHang from './ProblemHang.vue'
import ProblemClose from './ProblemClose.vue'
import ProblemBatchRedispatch from './ProblemBatchRedispatch.vue'
import CheckUserList from './CheckUserList.vue'
export default {
  components: {
    ProblemSolve,
    ProblemRecheck,
    ProblemValidate,
    ProblemRedispatch,
    CheckUserList,
    ProblemHang,
    ProblemClose,
    ProblemBatchRedispatch,
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
      // 人员选择
      userVisible: false,
      personnelType: '',
    }
  },
  inject: ['getProblemList'],
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
    isProblemTable() {
      return this.issueData.length !== 0
    },
  },

  created() {
    this.isMoblile()
  },
  methods: {
    customRow(record) {
      const { status } = record
      return {
        style: {
          color: status == 4 ? '#bfc5d1' : 'unset',
        },
      }
    },
    changePersonnel(value) {
      this.personnelType = value
    },
    // 人员选择
    checkUserArray(checkUser) {
      const perArray = checkUser.map((item) => {
        return item.name
      })
      if (this.personnelType == 'batcheExecutor') {
        this.$refs.batchRedispatchProblem.form.executor = Number(checkUser[0].id)
        this.$refs.batchRedispatchProblem.form.executorName = perArray.join()
        return
      }
      this.$refs.redispatchProblem.form.executor = Number(checkUser[0].id)
      this.$refs.redispatchProblem.form.executorName = perArray.join()
    },

    // 操作类型
    operationFilter(btnList) {
      // const { admintype } = this.$store.state.user // 获取用户类型 1是超级用户
      const operationArray = [
        {
          operName: '复制',
          operIcon: 'copy',
        },
        {
          operName: '编辑',
          operIcon: 'edit',
        },
        {
          operName: '详情',
          operIcon: 'file-done',
        },
        {
          operName: '分发',
          operIcon: 'select',
        },
        {
          operName: '转交',
          operIcon: 'export',
        },
        {
          operName: '解决',
          operIcon: 'question-circle',
        },
        {
          operName: '复核',
          operIcon: 'reconciliation',
        },
        {
          operName: '验证',
          operIcon: 'safety-certificate',
        },
        {
          operName: '关闭',
          operIcon: 'close-circle',
        },
        {
          operName: '挂起',
          operIcon: 'minus-circle',
        },
        {
          operName: '重开启',
          operIcon: 'key',
        },
        {
          operName: '催办',
          operIcon: 'bell',
        },
        {
          operName: '删除',
          operIcon: 'delete',
        },
      ]
      const newOperationList = []
      btnList?.forEach((item) => {
        newOperationList.push(operationArray[item])
      })
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
      switch (operName) {
        case '删除':
          this.problemDelectSend(record, '删除')
          break
        case '催办':
          this.problemDelectSend(record, '催办')
          break
        case '详情':
          this.$store.commit('SET_BACK_QP', this.$parent.$parent.$parent.queryParam)
          this.$router.push({
            path: '/problemInfo',
            query: { id: record.id },
          })
          break
        case '编辑':
          this.$store.commit('SET_BACK_QP', this.$parent.$parent.$parent.queryParam)
          this.$router.push({ path: '/problemAdd', query: { editId: record.id } })
          break
        case '复制':
          this.$router.push({ path: '/problemAdd', query: { editId: record.id, copyAdd: 1 } })
          break
        case '分发':
          this.$store.commit('SET_BACK_QP', this.$parent.$parent.$parent.queryParam)
          this.$router.push({ path: '/problemDistribure', query: { distributeId: record.id } })
          break
        case '解决':
          this.$refs.problemSolve.initSolv(record)
          break
        case '复核':
          this.$refs.recheckProblem.recheckForm(record)
          break
        case '验证':
          this.$refs.validateProblem.initValidate(record)
          break
        case '转交':
          this.$refs.redispatchProblem.initRedispatch(record)
          break
        case '挂起':
          this.$refs.hangProblem.initHang(record)
          break
        case '重开启':
          this.problemOpen(record)
          break
        case '关闭':
          this.$refs.closeProblem.initClose(record)
          break
        default:
          break
      }
    },

    // 删除\催办
    problemDelectSend(record, text) {
      const { id } = record
      const _this = this
      this.$confirm({
        content: '确定' + text + '该问题',
        onOk() {
          if (text == '删除') {
            IssueDelete({ id })
              .then((res) => {
                if (res.success) {
                  _this.$message.success(text + '成功')
                  _this.getProblemList()
                } else {
                  _this.$message.warning(res.message)
                }
              })
              .catch(() => {
                _this.$message.error('删除失败')
              })
          } else {
            IssueSendur({ id })
              .then((res) => {
                if (res.success) {
                  _this.$message.success(text + '成功')
                  _this.getProblemList()
                } else {
                  _this.$message.warning(res.message)
                }
              })
              .catch(() => {
                _this.$message.error('催办失败')
              })
          }
        },
        onCancel() {},
      })
    },

    //  重新开启
    problemOpen(record) {
      const { id } = record
      const _this = this
      this.$confirm({
        content: '确定重新开启',
        onOk() {
          IssueReOpen({ id })
            .then((res) => {
              if (res.success) {
                _this.$message.success('重开启成功')
                _this.getProblemList()
              } else {
                _this.$message.warning(res.message)
              }
            })
            .catch(() => {
              _this.$message.error('重开启失败')
            })
        },
        onCancel() {},
      })
    },

    // 选中转发
    checkForward() {
      const checkIssue = this.issueData.filter((item) => {
        return item.checkbox == true
      })
      const redispatchIssueList = checkIssue.filter((item) => {
        if (item.status == 0 || item.status == 3) {
          return item
        }
      })
      if (redispatchIssueList.length == 0) {
        this.$message.warning('所选问题不可转交')
      } else {
        this.$refs.batchRedispatchProblem.initRedispatch(redispatchIssueList)
      }
    },

    // 选中导出
    checkExport() {
      const exportData = this.issueData.filter((item) => item.checkbox)
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
.problem_table {
  /deep/ .ant-table-body {
    min-height: 360px;
  }
}
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
  /deep/.ant-dropdown {
    z-index: 1000;
  }
}
</style>