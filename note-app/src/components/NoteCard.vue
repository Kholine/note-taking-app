<template>
  <div 
    @click="$emit('view', note)"
    class="group relative bg-white border border-gray-200 rounded-lg hover:shadow-lg transition-all duration-200 cursor-pointer overflow-hidden"
  >
    <!-- Card content -->
    <div class="p-6">
      
      <!-- Header with title and actions -->
      <div class="flex items-start justify-between gap-3 mb-4">
        <h3 class="font-semibold text-gray-900 text-lg leading-tight line-clamp-2 flex-1">
          {{ note.title }}
        </h3>
        
        <!-- Action buttons - shown on hover -->
        <div class="flex items-center gap-1 opacity-0 group-hover:opacity-100 transition-opacity duration-200 flex-shrink-0">
          <button
            @click.stop="$emit('view', note)"
            class="p-1.5 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-md transition-colors"
            title="View note"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
            </svg>
          </button>
          
          <button
            @click.stop="$emit('edit', note)"
            class="p-1.5 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-md transition-colors"
            title="Edit note"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
          </button>
          
          <button
            @click.stop="$emit('delete', note)"
            class="p-1.5 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded-md transition-colors"
            title="Delete note"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
          </button>
        </div>
      </div>
      
      <!-- Content preview -->
      <div class="mb-4">
        <p class="text-gray-600 text-sm line-clamp-3 leading-relaxed">
          {{ note.content || 'No content' }}
        </p>
      </div>
      
      <!-- Footer metadata -->
      <div class="flex items-center justify-between text-xs text-gray-500">
        <span>{{ formatDateShort(note.updatedAt) }}</span>
        
        <!-- Color indicator -->
        <div :class="[
          'w-3 h-3 rounded-full',
          cardColors[colorIndex % cardColors.length]
        ]"></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Note } from '@/types'

const props = defineProps<{
  note: Note
  colorIndex: number
}>()

defineEmits<{
  view: [note: Note]
  edit: [note: Note]
  delete: [note: Note]
}>()

// Notion-inspired color palette - more subtle
const cardColors = [
  'bg-blue-500',
  'bg-purple-500', 
  'bg-green-500',
  'bg-orange-500',
  'bg-red-500',
  'bg-teal-500',
  'bg-indigo-500',
  'bg-pink-500',
  'bg-yellow-500',
  'bg-gray-500'
]

const formatDateShort = (dateString: string): string => {
  const date = new Date(dateString)
  const now = new Date()
  const diffInHours = Math.abs(now.getTime() - date.getTime()) / (1000 * 60 * 60)
  
  if (diffInHours < 1) {
    return 'Just now'
  } else if (diffInHours < 24) {
    return `${Math.floor(diffInHours)}h ago`
  } else if (diffInHours < 24 * 7) {
    const days = Math.floor(diffInHours / 24)
    return `${days}d ago`
  } else {
    return date.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric' 
    })
  }
}
</script>

<style scoped>
/* Line clamp utilities */
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.line-clamp-3 {
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style> 