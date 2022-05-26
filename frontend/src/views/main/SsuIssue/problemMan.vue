<!--
 * @Author: 林伟群
 * @Date: 2022-05-11 09:52:50
 * @LastEditTime: 2022-05-26 10:51:52
 * @LastEditors: 林伟群
 * @Description: 问题管理页面
 * @FilePath: \frontend\src\views\main\SsuIssue\problemMan.vue
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
import { SsuIssueColumnDis, IssuePage, IssueImport } from '@/api/modular/main/SsuIssueManage'

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
        {
          queryName: '待验证',
          key: 4,
        },
        {
          queryName: '未解决',
          key: 5,
        },
        {
          queryName: '已关闭',
          key: 6,
        },
        {
          queryName: '已挂起',
          key: 7,
        },
        // {
        //   queryName: '抄送给我',
        //   key: 8,
        // },
      ],
      query: 0, // 流程类型
      columns: [
        {
          title: '序号',
          align: 'center',
          sorter: true,
          dataIndex: 'id',
          width: '10em',
        },
        {
          title: '标题',
          align: 'center',
          sorter: true,
          dataIndex: 'title',
          width: '10em',
        },
        {
          title: '项目名',
          align: 'center',
          sorter: true,
          dataIndex: 'projectName',
          width: '10em',
        },
        {
          title: '产品名',
          align: 'center',
          sorter: true,
          dataIndex: 'productName',
          width: '10em',
        },
        {
          title: '问题模块',
          align: 'center',
          sorter: true,
          dataIndex: 'module',
          width: '10em',
          customRender: (text) => {
            const contentArray = this.$options.filters['dictData']('issue_module')
            const data = contentArray.find((item) => item.code == text)
            // const contentArray = ['研发', '研发', '试产', 'IQC', '量产', '售后']
            return data?.name
          },
        },
        {
          title: '问题性质',
          align: 'center',
          sorter: true,
          dataIndex: 'consequence',
          width: '10em',
          customRender: (text) => {
            // const contentArray = ['致命', '严重', '一般', '提示']
            const contentArray = this.$options.filters['dictData']('issue_consequence')
            const data = contentArray.find((item) => item.code == text)
            return data?.name
          },
        },
        {
          title: '问题分类',
          align: 'center',
          sorter: true,
          dataIndex: 'issueClassification',
          width: '10em',
          customRender: (text) => {
            // const contentArray = [
            //   '设计问题-硬件',
            //   '设计问题-产品软件',
            //   '设计问题-生产测试',
            //   ' 设计问题-结构',
            //   '设计问题-工艺',
            //   '设计问题-BOM ',
            //   '设计问题-器件',
            //   '设计问题-包装',
            //   '设计问题-治具',
            //   '制程问题-SMT',
            //   '制程问题-DIP ',
            //   '制程问题-工艺',
            //   '制程问题-设备',
            //   '制程问题-装配',
            //   '物料问题 ',
            //   '其它问题  ',
            // ]
            const contentArray = this.$options.filters['dictData']('issue_classification')
            const data = contentArray.find((item) => item.code == text)
            return data?.name
          },
        },
        {
          title: '问题来源',
          align: 'center',
          sorter: true,
          dataIndex: 'source',
          scopedSlots: { customRender: 'sourcescopedSlots' },
          width: '10em',
          customRender: (text) => {
            // const contentArray = ['客户反馈 ', '工厂 ', '测试发现 ']
            const contentArray = this.$options.filters['dictData']('issue_source')
            const data = contentArray.find((item) => item.code == text)
            return data?.name
          },
        },
        {
          title: '问题状态',
          align: 'center',
          sorter: true,
          dataIndex: 'status',
          width: '10em',
          customRender: (text) => {
            // const contentArray = ['已开启', '已分派', '已处理', '未解决', '已关闭', '已挂起']
            const contentArray = this.$options.filters['dictData']('issue_status')
            const data = contentArray.find((item) => item.code == text)
            return data?.name
          },
        },
        {
          title: '提出人',
          align: 'center',
          sorter: true,
          dataIndex: 'creatorName',
          width: '10em',
        },
        {
          title: '提出日期',
          align: 'center',
          sorter: true,
          dataIndex: 'createTime',
          width: '10em',
        },
        {
          title: '关闭日期',
          align: 'center',
          sorter: true,
          dataIndex: 'closeTime',
          width: '10em',
        },
        {
          title: '发现人',
          align: 'center',
          sorter: true,
          dataIndex: 'discoverName',
          width: '10em',
        },
        {
          title: '发现日期',
          align: 'center',
          sorter: true,
          dataIndex: 'discoverTime',
          width: '10em',
        },
        {
          title: '分发人',
          align: 'center',
          sorter: true,
          dataIndex: 'dispatcherName',
          width: '10em',
        },
        {
          title: '分发日期',
          align: 'center',
          sorter: true,
          dataIndex: 'dispatchTime',
          width: '10em',
        },
        {
          title: '预计完成日期',
          align: 'center',
          sorter: true,
          dataIndex: 'forecastSolveTime',
          width: '10em',
        },
        {
          title: '被抄送人',
          align: 'center',
          sorter: true,
          dataIndex: 'copyToName',
          width: '10em',
        },
        {
          title: '解决人',
          align: 'center',
          sorter: true,
          dataIndex: 'executorName',
          width: '10em',
        },
        {
          title: '解决日期',
          align: 'center',
          sorter: true,
          dataIndex: 'solveTime',
          width: '10em',
        },
        {
          title: '验证人',
          align: 'center',
          sorter: true,
          dataIndex: 'verifierName',
          width: '10em',
        },
        {
          title: '验证地点',
          align: 'center',
          sorter: true,
          dataIndex: 'verifierPlace',
          width: '10em',
        },
        {
          title: '验证日期',
          align: 'center',
          sorter: true,
          dataIndex: 'validateTime',
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
    console.log(this.$store.state)
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
      this.queryParam = { ...this.queryParam, ...value }
      this.getProblemList()
    },

    // 排序查询
    handleTableChange(value) {
      this.queryParam = { ...this.queryParam, ...value }
      this.getProblemList()
    },

    // 翻页
    queryProblem(value) {
      this.queryParam = { ...this.queryParam, ...value }
      this.getProblemList()
    },

    // 新增问题
    handleAddProblem() {
      this.$router.push({
        path: '/problemAdd',
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