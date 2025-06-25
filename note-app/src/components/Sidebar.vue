<script setup lang="ts">
  import { computed } from 'vue'
  const props = defineProps<{ searchQuery: string, notesCount: number }>()
  const emit = defineEmits(['update:searchQuery', 'create'])
  const searchQueryProxy = computed({
    get: () => props.searchQuery,
    set: (val: string) => emit('update:searchQuery', val)
  })
</script> 

<template>
  <div class="w-64 bg-gray-50 border-r border-gray-200 flex-shrink-0 flex flex-col h-full">
    <!-- Logo/Brand -->
    <div class="p-4 border-b border-gray-200 flex-shrink-0">
      <div class="flex items-center space-x-3">
        <div class="w-8 h-8 bg-gray-900 rounded-md flex items-center justify-center">
          <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
          </svg>
        </div>
        <div>
          <h1 class="text-lg font-semibold text-gray-900">Notes</h1>
          <p class="text-sm text-gray-500">{{ notesCount }} notes</p>
        </div>
      </div>
    </div>
    <!-- Search -->
    <div class="p-4 flex-shrink-0">
      <div class="relative">
        <svg class="w-4 h-4 absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
        </svg>
        <input
          v-model="searchQueryProxy"
          type="text"
          placeholder="Search notes..."
          class="w-full pl-10 pr-4 py-2 text-sm bg-white border border-gray-200 rounded-md text-gray-900 placeholder-gray-500 focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        />
      </div>
    </div>
    <!-- Navigation -->
    <nav class="flex-1 px-4 overflow-y-auto">
      <div class="space-y-1">
        <div class="flex items-center px-3 py-2 text-sm font-medium text-gray-900 bg-gray-100 rounded-md">
          <svg class="w-4 h-4 mr-3 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path>
          </svg>
          All Notes
        </div>
      </div>
    </nav>
    <!-- Add New Button -->
    <div class="p-4 flex-shrink-0 border-t border-gray-200">
      <button
        @click="$emit('create')"
        class="w-full bg-gray-900 hover:bg-gray-800 text-white font-medium py-2.5 px-4 rounded-md transition-colors flex items-center justify-center space-x-2 text-sm"
      >
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6"></path>
        </svg>
        <span>New Note</span>
      </button>
    </div>
  </div>
</template>
