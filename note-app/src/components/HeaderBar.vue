<script setup lang="ts">
  import { computed } from 'vue'
  const props = defineProps<{ title: string, sortBy: string, isDescending: boolean, showHamburger?: boolean }>()
  const emit = defineEmits(['update:sortBy', 'toggleSortDirection', 'toggleSidebar'])
  const sortByProxy = computed({
    get: () => props.sortBy,
    set: (val: string) => emit('update:sortBy', val)
  })
</script> 

<template>
  <header class="bg-white border-b border-gray-200 px-4 md:px-8 py-6 flex-shrink-0 flex items-center justify-between">
    <div class="flex items-center gap-3">
      <button v-if="showHamburger" @click="$emit('toggleSidebar')" class="md:hidden p-2 rounded-md hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-blue-500">
        <svg class="w-6 h-6 text-gray-900" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
        </svg>
      </button>
      <h1 class="text-3xl font-bold text-gray-900">{{ title }}</h1>
    </div>
    <div class="flex items-center space-x-3">
      <select
        v-model="sortByProxy"
        class="px-3 py-2 border border-gray-200 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent text-sm bg-white text-gray-900"
      >
        <option value="updatedAt">Recently Updated</option>
        <option value="createdAt">Recently Created</option>
        <option value="title">Title A-Z</option>
      </select>
      <button
        @click="$emit('toggleSortDirection')"
        class="p-2 border border-gray-200 rounded-md hover:bg-gray-50 text-gray-700"
        :title="isDescending ? 'Sort Ascending' : 'Sort Descending'"
      >
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path v-if="isDescending" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 14l-7 7m0 0l-7-7m7 7V3" />
          <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 10l7-7m0 0l7 7m-7-7v18" />
        </svg>
      </button>
    </div>
  </header>
</template>
