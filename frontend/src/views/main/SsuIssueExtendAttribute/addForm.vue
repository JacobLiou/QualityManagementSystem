<template>
  <a-modal
    title="新增问题扩展属性"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="模块编号" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择模块编号" v-decorator="['module', {rules: [{ required: true, message: '请选择模块编号！' }]}]">
            <a-select-option v-for="(item,index) in moduleData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="字段名" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入字段名" v-decorator="['attibuteName', {rules: [{required: true, message: '请输入字段名！'}]}]" />
        </a-form-item>
        <a-form-item label="字段代码" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入字段代码" v-decorator="['attributeCode', {rules: [{required: true, message: '请输入字段代码！'}]}]" />
        </a-form-item>
        <a-form-item label="字段值类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择字段值类型" v-decorator="['valueType', {rules: [{ required: true, message: '请选择字段值类型！' }]}]">
            <a-select-option v-for="(item,index) in valueTypeData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    SsuIssueExtendAttributeAdd
  } from '@/api/modular/main/SsuIssueExtendAttributeManage'

  export default {
    data () {
      return {
        labelCol: {
          xs: { span: 24 },
          sm: { span: 5 }
        },
        wrapperCol: {
          xs: { span: 24 },
          sm: { span: 15 }
        },
        moduleData: [],
        valueTypeData: [],
        visible: false,
        confirmLoading: false,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      add (record) {
        this.visible = true
        const moduleOption = this.$options
        this.moduleData = moduleOption.filters['dictData']('issue_module')
        const valueTypeOption = this.$options
        this.valueTypeData = valueTypeOption.filters['dictData']('code_gen_net_type')
      },
      /**
       * 提交表单
       */
      handleSubmit () {
        const { form: { validateFields } } = this
        this.confirmLoading = true
        validateFields((errors, values) => {
          if (!errors) {
            for (const key in values) {
              if (typeof (values[key]) === 'object') {
                values[key] = JSON.stringify(values[key])
              }
            }
            SsuIssueExtendAttributeAdd(values).then((res) => {
              if (res.success) {
                this.$message.success('新增成功')
                this.confirmLoading = false
                this.$emit('ok', values)
                this.handleCancel()
              } else {
                this.$message.error('新增失败：' + JSON.stringify(res.message))
              }
            }).finally((res) => {
              this.confirmLoading = false
            })
          } else {
            this.confirmLoading = false
          }
        })
      },
      handleCancel () {
        this.form.resetFields()
        this.visible = false
      }
    }
  }
</script>
