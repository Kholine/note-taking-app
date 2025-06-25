<script setup lang="ts">
  import { computed } from 'vue'
  const props = defineProps<{ message: string, type?: 'success' | 'error' | 'info', visible: boolean }>()
  const emit = defineEmits(['close'])
  const toastClass = computed(() => {
    if (props.type === 'success') return 'bg-green-50 border border-green-200'
    if (props.type === 'error') return 'bg-red-50 border border-red-200'
    return 'bg-white border border-gray-200'
  })
</script>

<template>
  <transition name="toast-fade">
    <div v-if="visible" :class="toastClass" class="fixed bottom-6 right-6 z-50 min-w-[240px] max-w-xs px-4 py-3 rounded-lg shadow-lg flex items-start space-x-3 animate-fade-in">
      <div>
        <span v-if="type === 'success'" class="text-green-500">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
        </span>
        <span v-else-if="type === 'error'" class="text-red-500">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
        </span>
        <span v-else class="text-blue-500">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2" fill="none" /><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01" /></svg>
        </span>
      </div>
      <div class="flex-1 text-sm text-gray-900">{{ message }}</div>
      <button @click="$emit('close')" class="ml-2 text-gray-400 hover:text-gray-700">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
      </button>
    </div>
  </transition>
</template>

<style scoped>
.toast-fade-enter-active, .toast-fade-leave-active {
  transition: opacity 0.3s, transform 0.3s;
}
.toast-fade-enter-from, .toast-fade-leave-to {
  opacity: 0;
  transform: translateY(20px);
}
.animate-fade-in {
  animation: fadeIn 0.3s ease;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
</style> 