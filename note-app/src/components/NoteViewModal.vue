<template>
  <!-- Modal Overlay -->
  <div 
    class="fixed inset-0 z-50 bg-black/60"
    @click="handleOverlayClick"
  >
    <!-- Modal Container -->
    <div class="fixed left-[50%] top-[50%] z-50 w-full max-w-4xl translate-x-[-50%] translate-y-[-50%] bg-white rounded-lg shadow-xl overflow-hidden h-[80vh] flex flex-col">
      
      <!-- Header -->
      <div class="border-b border-gray-200 p-6 flex-shrink-0">
        <div class="flex items-start justify-between gap-4">
          <div class="flex-1 min-w-0">
            <h1 class="text-2xl font-bold text-gray-900 break-words leading-tight mb-3">
              {{ note.title }}
            </h1>
            
            <!-- Metadata -->
            <div class="flex flex-wrap items-center gap-4 text-sm text-gray-600">
              <span>Created {{ formatDateShort(note.createdAt) }}</span>
              <span v-if="note.updatedAt !== note.createdAt">
                Updated {{ formatDateShort(note.updatedAt) }}
              </span>
            </div>
          </div>
          
          <button
            @click="$emit('close')"
            class="p-2 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-md transition-colors flex-shrink-0"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
      </div>

      <!-- Fixed Scrollable Content -->
      <div class="flex-1 overflow-y-auto p-6">
        <!-- Content Display -->
        <div v-if="note.content" class="prose prose-gray max-w-none">
          <div class="text-gray-900 leading-relaxed whitespace-pre-wrap text-base">
            {{ note.content }}
          </div>
        </div>
        
        <!-- Empty State -->
        <div v-else class="text-center py-12">
          <div class="text-5xl mb-4">📝</div>
          <h3 class="text-lg font-semibold text-gray-900 mb-2">This note is empty</h3>
          <p class="text-gray-600 mb-6">
            This note doesn't have any content yet. Click edit to add some content.
          </p>
          <button
            @click="$emit('edit', note)"
            class="bg-gray-900 text-white px-4 py-2 rounded-lg hover:bg-gray-800 transition-colors"
          >
            Add Content
          </button>
        </div>
      </div>

      <!-- Footer Actions -->
      <div class="flex justify-end items-center p-6 border-t border-gray-200 bg-gray-50 flex-shrink-0">
        
        <div class="flex space-x-3">
          <button
            @click="$emit('close')"
            class="px-4 py-2 text-gray-700 bg-white border border-gray-200 rounded-lg hover:bg-gray-50 transition-colors"
          >
            Close
          </button>
          
          <button
            @click="$emit('edit', note)"
            class="px-4 py-2 bg-gray-900 text-white rounded-lg hover:bg-gray-800 transition-colors flex items-center space-x-2"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
            <span>Edit</span>
          </button>
          
          <button
            @click="handleDelete"
            class="px-4 py-2 text-red-600 bg-red-50 border border-red-200 rounded-lg hover:bg-red-100 transition-colors flex items-center space-x-2"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
            <span>Delete</span>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import type { Note } from '@/types'

const props = defineProps<{
  note: Note
}>()

const emit = defineEmits<{
  close: []
  edit: [note: Note]
  delete: [note: Note]
  requestDelete: [note: Note]
}>()

const handleOverlayClick = (event: MouseEvent) => {
  if (event.target === event.currentTarget) {
    emit('close')
  }
}

const handleDelete = () => {
  emit('requestDelete', props.note)
}

const formatDateShort = (dateString: string): string => {
  // Parse as UTC and convert to local
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

// Handle Escape key to close modal
onMounted(() => {
  const handleEscape = (event: KeyboardEvent) => {
    if (event.key === 'Escape') {
      emit('close')
    }
  }
  
  document.addEventListener('keydown', handleEscape)
  
  // Cleanup
  return () => {
    document.removeEventListener('keydown', handleEscape)
  }
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

/* Prose styles for content */
.prose {
  max-width: none;
}

.prose p {
  margin-bottom: 1em;
}

.prose p:last-child {
  margin-bottom: 0;
}
</style> 