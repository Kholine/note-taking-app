<template>
  <!-- Modal Overlay -->
  <div 
    class="fixed inset-0 z-50 bg-black/60 flex items-center justify-center"
    @click="handleOverlayClick"
  >
    <!-- Notion-style Modal Container -->
    <div
      class="relative w-full max-w-3xl mx-auto bg-white rounded-2xl shadow-2xl overflow-hidden flex flex-col min-h-[60vh] max-h-[90vh] animate-fade-in-premium"
      @click.stop
    >
      <!-- Floating Actions (top right) -->
      <div class="absolute top-6 right-6 flex gap-2 z-10">
        <template v-if="mode === 'view'">
          <button
            @click="switchToEdit"
            class="p-2 text-gray-500 hover:text-gray-900 hover:bg-gray-100 rounded-full transition-colors"
            title="Edit"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
          </button>
          <button
            @click="handleDelete"
            class="p-2 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded-full transition-colors"
            title="Delete"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
          </button>
        </template>
        <button
          @click="handleCancel"
          class="p-2 text-gray-400 hover:text-gray-700 hover:bg-gray-100 rounded-full transition-colors"
          title="Close"
        >
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <!-- Main Content -->
      <div class="flex-1 overflow-y-auto px-8 py-10 sm:px-12 sm:py-14">
        <template v-if="mode === 'view'">
          <!-- Title -->
          <h1 class="text-4xl font-bold text-gray-900 mb-2 break-words leading-tight tracking-tight">
            {{ note?.title }}
          </h1>
          <!-- Metadata -->
          <div class="flex items-center gap-4 text-xs text-gray-400 mb-8">
            <span>Created {{ formatDateShort(note?.createdAt) }}</span>
            <span v-if="note && note.updatedAt !== note.createdAt">· Updated {{ formatDateShort(note.updatedAt) }}</span>
          </div>
          <!-- Divider -->
          <div class="h-px bg-gray-100 mb-8"></div>
          <!-- Content -->
          <div v-if="note?.content" class="prose prose-lg max-w-none text-gray-900 whitespace-pre-wrap">
            {{ note.content }}
          </div>
          <!-- Empty State -->
          <div v-else class="flex flex-col items-center justify-center py-16">
            <div class="text-5xl mb-4">📝</div>
            <h3 class="text-lg font-semibold text-gray-900 mb-2">This note is empty</h3>
            <p class="text-gray-600 mb-6">
              This note doesn't have any content yet. Click edit to add some content.
            </p>
            <button
              @click="switchToEdit"
              class="bg-gray-900 text-white px-4 py-2 rounded-lg hover:bg-gray-800 transition-colors"
            >
              Add Content
            </button>
          </div>
        </template>
        <template v-else>
          <!-- Edit Heading -->
          <div class="mb-8">
            <h2 class="text-2xl font-bold text-gray-900">
              {{ isEditing ? 'Edit Note' : 'New Note' }}
            </h2>
            <p class="text-sm text-gray-600 mt-1">
              {{ isEditing ? 'Make changes to your note' : 'Create a new note' }}
            </p>
          </div>
          <!-- Edit Form -->
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
                class="w-full px-4 py-3 text-gray-900 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder-gray-500"
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
                class="w-full px-4 py-3 text-gray-900 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-none placeholder-gray-500 min-h-[200px] flex-1"
                rows="12"
              />
            </div>
            <!-- Footer Actions -->
            <div class="flex justify-end gap-3 pt-4">
              <button
                type="button"
                @click="handleCancel"
                class="px-4 py-2 text-gray-700 bg-white border border-gray-200 rounded-lg hover:bg-gray-50 transition-colors"
              >
                Cancel
              </button>
              <button
                type="submit"
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
          </form>
        </template>
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
  delete: [note: Note]
}>()

const mode = ref<'view' | 'edit'>(props.note ? 'view' : 'edit')

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
    handleCancel()
  }
}

const handleCancel = () => {
  emit('cancel')
}

const handleDelete = () => {
  if (props.note) emit('delete', props.note)
}

const switchToEdit = () => {
  mode.value = 'edit'
  nextTick(() => {
    titleInput.value?.focus()
  })
}

const formatDateShort = (dateString?: string): string => {
  if (!dateString) return ''
  const date = new Date(dateString + 'Z');
  const now = new Date();
  const diffInHours = Math.abs(now.getTime() - date.getTime()) / (1000 * 60 * 60);
  if (diffInHours < 1) {
    return 'just now';
  } else if (diffInHours < 24) {
    return `${Math.floor(diffInHours)}h ago`;
  } else if (diffInHours < 24 * 7) {
    const days = Math.floor(diffInHours / 24);
    return `${days}d ago`;
  } else {
    return date.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric',
      year: 'numeric'
    });
  }
}

// Initialize form data when editing
onMounted(() => {
  if (props.note) {
    formData.value = {
      title: props.note.title,
      content: props.note.content
    }
  }
})
</script>

<style scoped>
/* Custom scrollbar for modal content */
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

.prose {
  max-width: none;
}

/* Responsive modal width */
@media (max-width: 640px) {
  .max-w-3xl {
    max-width: 98vw !important;
    border-radius: 1rem !important;
    padding-left: 0.5rem !important;
    padding-right: 0.5rem !important;
  }
}
</style> 