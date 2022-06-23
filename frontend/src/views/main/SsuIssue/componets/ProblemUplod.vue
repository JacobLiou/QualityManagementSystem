<!--
 * @Author: 林伟群
 * @Date: 2022-05-31 09:45:03
 * @LastEditTime: 2022-06-23 10:31:11
 * @LastEditors: 林伟群
 * @Description: 文件上传
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ProblemUplod.vue
-->
<template>
  <section>
    <a-upload :customRequest="customRequest" :multiple="true" :showUploadList="true" name="file" @change="handleChange">
      <a-button icon="upload">附件上传</a-button>
    </a-upload>
  </section>
</template>

<script>
import { sysFileInfoUpload } from '@/api/modular/system/fileManage'
export default {
  props: ['type'],
  data() {
    return {
      attachment: [],
      uploadInfo: {},
    }
  },
  methods: {
    handleChange(info) {
      if (info.file.status === 'removed') {
        const fileIndex = this.uploadInfo.fileList.findIndex((item) => {
          if (item.uid === info.file.uid) {
            return item
          }
        })
        this.attachment.splice(fileIndex, 1)
      }
      this.uploadInfo = info
    },
    // 附件上传
    customRequest(data) {
      const { file } = data
      const formData = new FormData()
      formData.append('file', file)
      sysFileInfoUpload(formData).then((res) => {
        if (res.success) {
          this.$message.success('附件上传成功')
          this.uploadInfo.file.status = 'done'
          const attachment = {
            attachmentId: res.data,
            fileName: file.name,
            attachmentType: Number(this.type),
          }
          this.attachment.push(attachment)
          this.$emit('uploadProblem', this.attachment)
        } else {
          this.$message.error('上传失败：' + res.message)
          this.uploadInfo.file.status = 'error'
        }
      })
    },
  },
}
</script>

<style>
</style>