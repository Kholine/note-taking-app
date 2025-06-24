import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useToastStore = defineStore('toast', () => {
  const message = ref('')
  const type = ref<'success' | 'error' | 'info'>('info')
  const visible = ref(false)
  let timeout: ReturnType<typeof setTimeout> | null = null

  function showToast(msg: string, t: 'success' | 'error' | 'info' = 'info') {
    message.value = msg
    type.value = t
    visible.value = true
    if (timeout) clearTimeout(timeout)
    timeout = setTimeout(() => {
      visible.value = false
    }, 3000)
  }

  function hideToast() {
    visible.value = false
    if (timeout) clearTimeout(timeout)
  }

  return { message, type, visible, showToast, hideToast }
}) 