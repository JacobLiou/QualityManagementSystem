<template>
  <a-modal
    :title="titleName"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="人员组名称" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入人员组名称"
            v-decorator="['groupName', { rules: [{ required: true, message: '请输入人员组名称' }] }]"
          />
        </a-form-item>
        <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input-number
            placeholder="请输入排序"
            :min="0"
            style="width: 100%"
            v-decorator="['sort', { rules: [{ required: true, message: '请输入排序数值' }] }]"
          />
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="人员关联">
          <a-transfer
            v-decorator="['userIdList']"
            :data-source="mockData"
            show-search
            :filter-option="filterOption"
            :target-keys="targetKeys"
            :render="(item) => item.title"
            @change="handleChange"
            @search="handleSearch"
            :list-style="{
              width: '40%',
              height: '300px',
            }"
          />
        </a-form-item>
        <a-form-item v-if="optionType == 'edit'"><span v-decorator="['id']" v-show="false"></span></a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { SsuGroupAdd, SsuGroupEdit, getfuzzyusers } from '@/api/modular/main/SsuGroupManage'
export default {
  data() {
    return {
      labelCol: {
        xs: { span: 24 },
        sm: { span: 5 },
      },
      wrapperCol: {
        xs: { span: 24 },
        sm: { span: 15 },
      },
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      mockData: [],
      targetKeys: [],
      optionType: 'add', // 操作类型
      titleName: '新增人员组',
    }
  },

  methods: {
    // 初始化方法,新增/编辑/复制
    AEC(record, type = 'add') {
      this.visible = true
      this.optionType = type
      const userList = record?.userList
      let newUserIdList = this.editUserList(userList)
      switch (type) {
        case 'edit':
          setTimeout(() => {
            this.form.setFieldsValue({
              id: record.id,
              groupName: record.groupName,
              sort: record.sort,
              userIdList: newUserIdList.map((item) => item.id),
            })
          }, 100)
          this.titleName = '编辑人员组'
          break
        case 'copy':
          setTimeout(() => {
            this.form.setFieldsValue({
              groupName: record.groupName,
              sort: record.sort,
              userIdList: newUserIdList.map((item) => item.id),
            })
          }, 100)
          this.titleName = '新增人员组'
          break
        default:
          setTimeout(() => {
            this.form.setFieldsValue({
              userIdList: [],
            })
          }, 100)
          break
      }
    },
    // 编辑关联人员列表处理
    editUserList(userList) {
      if (!userList?.length) return []
      this.mockData = userList.map((item) => {
        const data = {
          key: item.id,
          title: item.name,
          description: item.name,
          chosen: true,
        }
        return data
      })
      this.targetKeys = this.mockData.filter((item) => item.chosen == true).map((item) => item.key)
      const newUserIdList = userList.map((item) => {
        return {
          id: item.id,
          name: item.name,
        }
      })
      return newUserIdList
    },
    filterOption(inputValue, option) {
      return option.description.indexOf(inputValue) > -1
    },
    handleChange(targetKeys, direction, moveKeys) {
      this.targetKeys = targetKeys
      console.log(this.targetKeys)
      this.form.setFieldsValue({
        userIdList: this.targetKeys,
      })
      console.log(this.form.getFieldsValue())
    },
    // 搜索框变化的数据
    handleSearch(dir, value) {
      if (dir == 'left') {
        // 远程搜索获取数据
        getfuzzyusers({ name: value }).then((res) => {
          if (res.success) {
            const data = res.data.map((item, i) => {
              const data = {
                key: item.id,
                title: item.name,
                description: item.name,
                chosen: false,
              }
              return data
            })
            const newData = data.map((item) => JSON.stringify(item))
            const mockData = this.mockData.map((item) => JSON.stringify(item))
            const newMockData = [...new Set([...mockData, ...newData])]
            this.mockData = newMockData.map((item) => JSON.parse(item))
          }
        })
        this.targetKeys = this.targetKeys
      }
    },
    /**
     * 提交表单
     */
    handleSubmit() {
      const {
        form: { validateFields },
      } = this
      this.confirmLoading = true
      validateFields((errors, values) => {
        if (!errors) {
          if (this.optionType == 'edit') {
            this.EGroup(values)
          } else {
            this.ACGroup(values)
          }
        } else {
          this.confirmLoading = false
        }
      })
    },
    // 新增/复制人员组
    ACGroup(values) {
      SsuGroupAdd(values)
        .then((res) => {
          if (res.success) {
            this.$message.success('新增成功')
            this.confirmLoading = false
            this.$emit('ok', values)
            this.handleCancel()
          } else {
            this.$message.error('新增失败：' + JSON.stringify(res.message))
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },
    // 编辑人员组
    EGroup(values) {
      SsuGroupEdit(values)
        .then((res) => {
          if (res.success) {
            this.$message.success('编辑成功')
            this.confirmLoading = false
            this.$emit('ok', values)
            this.handleCancel()
          } else {
            this.$message.error('编辑失败：' + JSON.stringify(res.message))
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },
    handleCancel() {
      this.form.resetFields()
      this.mockData = []
      this.targetKeys = []
      this.visible = false
    },
  },
}
</script>
