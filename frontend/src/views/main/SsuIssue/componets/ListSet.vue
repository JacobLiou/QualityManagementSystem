<!--
 * @Author: 林伟群
 * @Date: 2022-05-12 13:39:00
 * @LastEditTime: 2022-05-13 16:49:19
 * @LastEditors: 林伟群
 * @Description: 列设置弹窗
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ListSet.vue
-->
<template>
  <div class="ant-dropdown-menu s-tool-column ant-dropdown-content">
    <div class="s-tool-column-header s-tool-column-item">
      <a-checkbox :indeterminate="indeterminate" :checked="checkAll" @change="CheckAllChange"> 列展示 </a-checkbox>
      <a @click="resetList">重置</a>
    </div>
    <a-divider />
    <div class="ant-checkbox-group">
      <div>
        <draggable v-model="columnsSetting" animation="300" @end="emitColumnChange">
          <div class="s-tool-column-item" v-for="item in columnsSetting" :key="item.title">
            <div class="s-tool-column-handle">
              <a-icon type="more" />
              <a-icon type="more" />
            </div>
            <a-checkbox v-model="item.checked" @change="ChangeItem">{{ item.title }}</a-checkbox>
          </div>
        </draggable>
      </div>
    </div>
  </div>
</template>

<script>
import draggable from 'vuedraggable'
import { SsuIssueColumnDis, SsuIssueColumnUpdate } from '@/api/modular/main/SsuIssueManage'

export default {
  props: {
    columns: {
      type: Array,
      default: () => [],
    },
  },
  components: {
    draggable,
  },
  data() {
    return {
      visible: false,
      indeterminate: false,
      checkAll: false,
      columnsSetting: [],
      originColumns: [],
    }
  },
  created() {
    this.getColumnDis()
  },
  methods: {
    // 获取列表数据
    async getColumnDis() {
      const columnRes = await SsuIssueColumnDis()
      if (columnRes.code == 200) {
        const columnObject = columnRes.data
        this.columnsSetting = this.columns.map((item) => {
          if (columnObject[item.dataIndex] == undefined) {
            item = { ...item, checked: false }
          } else {
            item = { ...item, checked: true }
          }
          return item
        })
      } else {
        this.columnsSetting = this.columns.map((value) => ({ ...value, checked: true }))
      }
      this.checkAllState(this.columnsSetting)
      this.originColumns = JSON.parse(JSON.stringify(this.columnsSetting))
    },
    resetList() {
      this.columnsSetting = JSON.parse(JSON.stringify(this.originColumns))
      this.checkAllState(this.columnsSetting)
      this.emitColumnChange()
    },
    ChangeItem() {
      this.checkAllState(this.columnsSetting)
      this.updateColumn(this.columnsSetting)
      this.emitColumnChange()
    },
    CheckAllChange() {
      this.indeterminate = false
      this.checkAll = !this.checkAll
      this.columnsSetting.forEach((item) => (item.checked = this.checkAll))
      this.updateColumn(this.columnsSetting)
      this.emitColumnChange()
    },
    emitColumnChange() {
      this.$emit('columnChange', this.columnsSetting)
    },

    // 设置列表
    async updateColumn(columnsSetting) {
      let parameter = {}
      columnsSetting.forEach((item) => {
        if (item.checked) {
          parameter[`${item.dataIndex}`] = item.title
        }
      })
      await SsuIssueColumnUpdate(parameter) // 设置列表
    },

    // 控制全选按钮的显示状态
    checkAllState(columnsSetting) {
      const checkedList = columnsSetting.filter((item) => item.checked)
      this.indeterminate = !!checkedList.length && checkedList.length < columnsSetting.length
      this.checkAll = checkedList.length === columnsSetting.length
    },
  },
}
</script>

<style lang="less" scoped>
.table-wrapper {
  background: #fff;
}
.s-table-tool {
  display: flex;
  margin-bottom: 16px;
  .s-table-tool-left {
    flex: 1;
  }
  .s-table-tool-right {
    display: inline-flex;
    align-items: center;
    .s-tool-item {
      font-size: 16px;
      margin-left: 16px;
      cursor: pointer;
    }
  }
}

.s-tool-column-item {
  display: flex;
  align-items: center;
  padding: 4px 16px 4px 4px;
  .ant-checkbox-wrapper {
    flex: 1;
  }
  .s-tool-column-handle {
    opacity: 0.8;
    cursor: move;
    .anticon-more {
      font-size: 12px;
      margin-top: 2px;
      & + .anticon-more {
        margin: 2px 4px 0 -8px;
      }
    }
  }
}
.s-tool-column-header {
  padding: 5px 16px 10px 24px;
  min-width: 180px;
}
.s-tool-column {
  .ant-divider {
    margin: 0;
  }
  .ant-checkbox-group {
    padding: 4px 0;
    display: block;
  }
}
</style>
