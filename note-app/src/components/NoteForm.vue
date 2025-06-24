<template>
  <form @submit.prevent="handleSubmit" class="space-y-6 h-full flex flex-col">
    <!-- Title Field -->
    <div>
      <label for="title" class="block text-sm font-medium text-gray-900 mb-2">
        Title
      </label>
      <input
        id="title"
        ref="titleInput"
        v-model="formData.title"
        type="text"
        required
        placeholder="Note title..."
        class="w-full px-4 py-3 text-gray-900 border border-gray-200 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder-gray-500"
      />
      <div v-if="titleError" class="text-sm text-red-600 mt-1">
        {{ titleError }}
      </div>
    </div>
    <!-- Content Field -->
    <div class="flex-1 flex flex-col pb-10">
      <label for="content" class="block text-sm font-medium text-gray-900 mb-2">
        Content
      </label>
      <textarea
        id="content"
        v-model="formData.content"
        placeholder="Start writing..."
        class="w-full px-4 py-3 text-gray-900 border border-gray-200 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-none placeholder-gray-500 min-h-[200px] flex-1"
        rows="12"
      />
    </div>
    <slot name="footer" />
  </form>
</template>
<script setup lang="ts">
import { ref, onMounted, nextTick } from 'vue'
const props = defineProps<{
  formData: { title: string; content: string }
  titleError: string
  isEditing: boolean
}>()
const emit = defineEmits(['save', 'cancel'])
const titleInput = ref<HTMLInputElement>()
const handleSubmit = () => {
  emit('save')
}
onMounted(() => {
  nextTick(() => {
    titleInput.value?.focus()
  })
})
</script> 