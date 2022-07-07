<!--
 * @Author: 林伟群
 * @Date: 2022-05-11 09:52:50
 * @LastEditTime: 2022-06-23 10:38:27
 * @LastEditors: 林伟群
 * @Description: 问题管理页面
 * @FilePath: \frontend\src\views\main\SsuIssue\index.vue
-->
<template>
  <section class="prolem">
    <ProblemSelect @queryParamSelect="queryParamSelect"></ProblemSelect>
    <a-card class="problem_list">
      <a-row :gutter="[24, 12]" align="middle" type="flex">
        <a-col :xxl="2" :xl="4" :md="4" :xs="6" @click="handleQuery(item.key)" v-for="item in queryList" :key="item.key"
          ><div class="list_btn" :class="{ active_list: queryParam.QueryCondition == item.key }">
            {{ item.queryName }}
          </div>
        </a-col>
        <a-col :xxl="2" :xl="4" :md="4" :xs="6" class="list_button">
          <a-button type="primary" @click="handleAddProblem">新增</a-button></a-col
        >
        <a-col :xxl="2" :xl="4" :md="4" :xs="6" class="list_button">
          <a-button @click="templateFile">
            <a-tooltip title="模板下载" placement="top">
              <a-icon type="download" />
              模板下载
            </a-tooltip></a-button
          ></a-col
        >
        <a-col :xxl="2" :xl="4" :md="4" :xs="6" class="list_button">
          <a-upload :customRequest="customRequest" :multiple="true" :showUploadList="false" name="file">
            <a-button icon="upload">导入</a-button>
          </a-upload>
          <!-- <a-button @click="handleImport">导入</a-button> -->
        </a-col>
        <a-col :xxl="2" :xl="2" :md="4" :xs="6" class="list_button">
          <a-dropdown :trigger="['click']" :getPopupContainer="(triggerNode) => triggerNode.parentNode">
            <img
              class="image_img"
              src="../../../assets/image/设置.png"
              title="列设置"
              @click="(e) => e.preventDefault()"
            />
            <ListSet slot="overlay" :columns="columns" @columnChange="columnChange"></ListSet>
          </a-dropdown>
        </a-col>
      </a-row>
      <div class="problem_tab">
        <a-spin :spinning="spinning">
          <Table
            :columns="columnsShow"
            :issueList="issueList"
            :queryParam="queryParam"
            :totalNum="totalNum"
            @queryProblem="queryProblem"
            @handleTableChange="handleTableChange"
          ></Table>
        </a-spin>
      </div>
    </a-card>
  </section>
</template>

<script>
import ProblemSelect from './componets/ProblemSelect.vue'
import ListSet from './componets/ListSet.vue'
import Table from './componets/Table.vue'
import {
  SsuIssueColumnDis,
  IssuePage,
  IssueImport,
  IssueTemplate,
  Downloadfile,
} from '@/api/modular/main/SsuIssueManage'

export default {
  components: { ProblemSelect, ListSet, Table },
  data() {
    return {
      queryList: [
        {
          queryName: '所有',
          key: 0,
        },
        {
          queryName: '由我创建',
          key: 1,
        },
        {
          queryName: '指派给我',
          key: 2,
        },
        {
          queryName: '由我解决',
          key: 3,
        },
      ],
      query: 0, // 流程类型
      columns: [
        {
          title: '序号',
          align: 'left',
          sorter: true,
          dataIndex: 'serialNumber',
          width: '10em',
        },
        {
          title: '标题',
          align: 'left',
          sorter: true,
          dataIndex: 'title',
          width: '20em',
          customRender: (text, row, index) => {
            const color = row.status == 4 ? '#bfc5d1' : '#2F66F9'
            const openDrawer = () => {
              this.$store.commit('SET_BACK_QP', this.queryParam)
              this.$router.push({
                path: '/problemInfo',
                query: { id: row.id },
              })
            }
            return (
              <a
                href="javascript:;"
                style={'color:' + color + '; display: inline-block; width: 100%; height: 100%'}
                onClick={openDrawer}
                title={text}
                class="clickOutSideClass"
              >
                {text}
              </a>
            )
          },
        },
        {
          title: '状态',
          align: 'left',
          sorter: true,
          dataIndex: 'status',
          width: '6em',
          customRender: (text) => {
            const contentArray = this.$options.filters['dictData']('issue_status')
            const data = contentArray.find((item) => item.code == text)
            const colorArray = ['#1890ff', '#f8b37c', '#229342', '#ff5500', '#bfc5d1', '#f1d710', '#d2b4e0', '#94b0f0']
            return <a-tag color={colorArray[text]}>{data?.name}</a-tag>
          },
        },
        {
          title: '待办人',
          align: 'left',
          sorter: false,
          dataIndex: 'currentAssignName',
          width: '8em',
        },
        {
          title: '项目',
          align: 'left',
          sorter: false,
          dataIndex: 'projectName',
          width: '8em',
        },
        {
          title: '产品',
          align: 'left',
          sorter: false,
          dataIndex: 'productName',
          width: '8em',
        },
        {
          title: '模块',
          align: 'left',
          sorter: true,
          dataIndex: 'module',
          width: '6em',
          customRender: (text) => {
            const contentArray = this.$options.filters['dictData']('issue_module')
            const data = contentArray.find((item) => item.code == text)
            return data?.name
          },
        },
        {
          title: '性质',
          align: 'left',
          sorter: true,
          dataIndex: 'consequence',
          width: '6em',
          customRender: (text) => {
            const contentArray = this.$options.filters['dictData']('issue_consequence')
            const data = contentArray.find((item) => item.code == text)
            return data?.name
          },
        },
        {
          title: '问题分类',
          align: 'left',
          sorter: true,
          dataIndex: 'issueClassification',
          width: '10em',
          customRender: (text) => {
            const contentArray = this.$options.filters['dictData']('issue_classification')
            const data = contentArray.find((item) => item.code == text)
            return data?.name
          },
        },
        {
          title: '来源',
          align: 'left',
          sorter: true,
          dataIndex: 'source',
          scopedSlots: { customRender: 'sourcescopedSlots' },
          width: '8em',
          customRender: (text) => {
            const contentArray = this.$options.filters['dictData']('issue_source')
            const data = contentArray.find((item) => item.code == text)
            return data?.name
          },
        },

        {
          title: '提出人',
          align: 'left',
          sorter: false,
          dataIndex: 'creatorName',
          width: '6em',
        },
        {
          title: '提出时间',
          align: 'center',
          sorter: true,
          dataIndex: 'createTime',
          width: '8em',
        },

        {
          title: '发现人',
          align: 'left',
          sorter: false,
          dataIndex: 'discoverName',
          width: '6em',
        },
        {
          title: '发现时间',
          align: 'center',
          sorter: true,
          dataIndex: 'discoverTime',
          width: '8em',
        },
        {
          title: '分发人',
          align: 'left',
          sorter: false,
          dataIndex: 'dispatcherName',
          width: '6em',
        },
        {
          title: '分发时间',
          align: 'center',
          sorter: true,
          dataIndex: 'dispatchTime',
          width: '8em',
        },
        {
          title: '预计完成日期',
          align: 'center',
          sorter: true,
          dataIndex: 'forecastSolveTime',
          width: '10em',
        },
        {
          title: '解决人',
          align: 'left',
          sorter: false,
          dataIndex: 'executorName',
          width: '6em',
        },
        {
          title: '解决时间',
          align: 'center',
          sorter: true,
          dataIndex: 'solveTime',
          width: '8em',
        },
        {
          title: '验证人',
          align: 'left',
          sorter: false,
          dataIndex: 'verifierName',
          width: '6em',
        },
        {
          title: '验证地点',
          align: 'left',
          sorter: true,
          dataIndex: 'verifierPlace',
          width: '10em',
        },
        {
          title: '验证时间',
          align: 'center',
          sorter: true,
          dataIndex: 'validateTime',
          width: '8em',
        },
        {
          title: '实际完成时间',
          align: 'center',
          sorter: true,
          dataIndex: 'closeTime',
          width: '10em',
        },
      ],
      columnsShow: [],
      columnsList: {}, // 获取的列表
      issueList: [],
      queryParam: {
        PageNo: 1,
        PageSize: 20,
        QueryCondition: 0,
      }, // 搜索参数
      totalNum: 20,
      spinning: false,
      fileObj: '',
    }
  },
  created() {
    this.getColumnDis()
    console.log(this.$store.state, 'store')
  },
  provide() {
    return { getProblemList: this.getProblemList }
  },
  methods: {
    // 获取列表标题数据
    async getColumnDis() {
      try {
        const columnRes = await SsuIssueColumnDis()
        let columnsShow = []
        this.spinning = true
        if (columnRes.code == 200) {
          this.columnsList = columnRes.data
          columnsShow = this.columns.filter((item) => {
            if (this.columnsList[item.dataIndex]) {
              return item
            }
          })
        } else {
          columnsShow = this.columns
        }
        this.columnsShow = this.changeColumnsShow(columnsShow)
        this.getProblemList()
      } catch (error) {
        this.spinning = false
      }
    },

    // 表头显示
    columnChange(columnsSetting) {
      const columnsShow = columnsSetting.filter((item) => {
        if (item.checked) {
          return item
        }
      })
      this.columnsShow = this.changeColumnsShow(columnsShow)
    },

    // 改造展示的columnsShow
    changeColumnsShow(columnsShow) {
      columnsShow.push({
        title: '操作', // 操作
        dataIndex: 'operation',
        key: 'operation',
        fixed: 'right',
        align: 'center',
        scopedSlots: { customRender: 'operation' },
      })
      columnsShow.unshift({
        dataIndex: 'checkbox',
        key: 'checkbox',
        width: 48,
        scopedSlots: { customRender: 'checkbox' },
      })
      return columnsShow
    },

    // 获取问题列表数据
    async getProblemList() {
      try {
        this.spinning = true
        const { backQueryParam } = this.$store.state.record
        if (backQueryParam.PageSize) {
          this.queryParam = backQueryParam
          this.$store.commit('SET_BACK_QP', {})
        }
        const problemRes = await IssuePage(this.queryParam)
        if (problemRes.code == 200) {
          this.totalNum = problemRes.data.totalRows
          this.issueList = problemRes.data.rows
        } else {
          this.$message.warning('查询失败')
          this.issueList = []
        }
        this.spinning = false
      } catch (error) {
        this.spinning = false
      }
    },

    // 条件查询
    handleQuery(value) {
      this.queryParam.QueryCondition = value
      this.getProblemList()
    },

    // 筛选查询
    queryParamSelect(value) {
      console.log(value)
      this.queryParam = { ...this.queryParam, ...value }
      console.log(this.queryParam)
      this.getProblemList()
    },

    // 排序查询
    handleTableChange(value) {
      this.queryParam = { ...this.queryParam, ...value }
      this.getProblemList()
    },

    // 翻页
    queryProblem(value) {
      console.log(value)
      this.queryParam = { ...this.queryParam, ...value }
      this.getProblemList()
    },

    // 新增问题
    handleAddProblem() {
      this.$router.push({
        path: '/problemAdd',
      })
    },

    // 模板下载
    templateFile() {
      IssueTemplate()
        .then((res) => {
          this.confirmLoading = false
          Downloadfile(res)
          // eslint-disable-next-line handle-callback-err
        })
        .catch((err) => {
          this.confirmLoading = false
          this.$message.error('下载错误：获取文件流错误' + err)
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },

    // 导入
    customRequest(data) {
      this.fileObj = data.file

      if (this.fileObj) {
        const formData = new FormData()
        formData.append('file', this.fileObj)
        // 0：正常附件 1：问题详情富文本 2：原因分析富文本 3：解决措施富文本 4：验证情况富文本
        formData.append('attachmentType', '0')
        IssueImport(formData).then((res) => {
          if (res.success) {
            this.$message.success('导入成功')
            this.fileObj = ''
            this.getProblemList()
          } else {
            this.$message.error('导入失败：' + res.message)
          }
        })
      }
    },
  },
}
</script>

<style lang="less" scoped>
.list_btn {
  cursor: pointer;
  text-align: center;
  font-size: 1.1em;
  &:hover {
    font-weight: 700;
    &::before {
      content: '';
      position: absolute;
      width: 80%;
      height: 2px;
      bottom: 0;
      left: 10%;
      background: #e6f7ff;
    }
  }
}
.list_button {
  text-align: center;
  font-size: 1.1em;
}
.active_list {
  color: #1890ff;
  font-weight: 700;
  &::before {
    content: '';
    position: absolute;
    width: 80%;
    height: 2px;
    bottom: 0;
    left: 10%;
    background: #1890ff;
  }
}
.image_img {
  cursor: pointer;
  width: 2em;
}
.list_set {
  position: absolute;
  top: 0;
  right: 0;
  width: 180px;
  z-index: 999;
}
.problem_tab {
  margin-top: 1.5em;
}
</style>