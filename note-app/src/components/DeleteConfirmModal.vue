<script setup lang="ts">
  defineEmits(['cancel', 'confirm'])
</script>   

<template>
  <div class="fixed inset-0 z-50 bg-black/60 flex items-center justify-center">
    <div class="bg-white rounded-lg shadow-xl max-w-sm w-full p-6">
      <h2 class="text-lg font-semibold text-gray-900 mb-2">Confirm Delete</h2>
      <p class="text-gray-700 mb-6">Are you sure you want to delete this note? This action cannot be undone.</p>
      <div class="flex justify-end space-x-3">
        <button @click="$emit('cancel')" class="px-4 py-2 bg-white border border-gray-200 rounded-lg text-gray-700 hover:bg-gray-50 transition-colors">Cancel</button>
        <button @click="$emit('confirm')" class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors">Delete</button>
      </div>
    </div>
  </div>
</template>

