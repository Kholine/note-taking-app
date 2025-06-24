<template>
  <!-- Modal Overlay -->
  <div 
    class="fixed inset-0 z-50 bg-black/60"
    @click="handleOverlayClick"
  >
    <!-- Modal Container -->
    <div class="fixed left-[50%] top-[50%] z-50 w-full max-w-4xl translate-x-[-50%] translate-y-[-50%] bg-white rounded-lg shadow-xl overflow-hidden h-[80vh] flex flex-col">
      
      <!-- Header -->
      <div class="border-b border-gray-200 p-6 flex-shrink-0 flex items-center justify-between">
        <div>
          <h2 class="text-2xl font-bold text-gray-900">
            {{ isEditing ? 'Edit Note' : 'New Note' }}
          </h2>
          <p class="text-sm text-gray-600 mt-1">
            {{ isEditing ? 'Make changes to your note' : 'Create a new note' }}
          </p>
        </div>
        <button
          @click="$emit('cancel')"
          class="p-2 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-md transition-colors flex-shrink-0"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <!-- Fixed Scrollable Content -->
      <div class="flex-1 overflow-y-auto p-6">
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

        </form>
      </div>

      <!-- Footer Actions -->
      <div class="flex justify-end items-center p-6 border-t border-gray-200 bg-gray-50 flex-shrink-0">
        <div class="flex space-x-3">
          <button
            type="button"
            @click="$emit('cancel')"
            class="px-4 py-2 text-gray-700 bg-white border border-gray-200 rounded-lg hover:bg-gray-50 transition-colors"
          >
            Cancel
          </button>
          <button
            type="submit"
            @click="handleSubmit"
            :disabled="!isFormValid"
            class="px-4 py-2 bg-gray-900 text-white rounded-lg hover:bg-gray-800 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors flex items-center space-x-2"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path v-if="!isEditing" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
              <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
            </svg>
            <span>{{ isEditing ? 'Save Changes' : 'Create Note' }}</span>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, nextTick, watch } from 'vue'
import type { Note, CreateNoteRequest, UpdateNoteRequest } from '@/types'

const props = defineProps<{
  note?: Note | null
}>()

const emit = defineEmits<{
  save: [noteData: CreateNoteRequest | UpdateNoteRequest]
  cancel: []
}>()

const titleInput = ref<HTMLInputElement>()

const formData = ref({
  title: '',
  content: ''
})

const titleError = ref('')

const isEditing = computed(() => !!props.note)

const isFormValid = computed(() => {
  return formData.value.title.trim().length > 0 && !titleError.value
})

// Validation
watch(() => formData.value.title, (newTitle) => {
  if (newTitle.trim().length === 0) {
    titleError.value = 'Title is required'
  } else if (newTitle.trim().length > 100) {
    titleError.value = 'Title must be less than 100 characters'
  } else {
    titleError.value = ''
  }
})

const handleSubmit = () => {
  if (!isFormValid.value) return
  
  emit('save', {
    title: formData.value.title.trim(),
    content: formData.value.content.trim()
  })
}

const handleOverlayClick = (event: MouseEvent) => {
  if (event.target === event.currentTarget) {
    emit('cancel')
  }
}

const formatDate = (dateString: string): string => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleString('en-US', { 
    month: 'short', 
    day: 'numeric',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

// Initialize form data when editing
onMounted(async () => {
  if (props.note) {
    formData.value = {
      title: props.note.title,
      content: props.note.content
    }
  }
  nextTick(() => {
    titleInput.value?.focus()
  })
})
</script>

<style scoped>
/* Custom scrollbar */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #f8f9fa;
}

::-webkit-scrollbar-thumb {
  background: #e9ecef;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #dee2e6;
}
</style> 