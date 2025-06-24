<template>
  <div class="h-screen w-screen bg-white flex overflow-hidden">
    <!-- Left Sidebar - Notion Style -->
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
            <p class="text-sm text-gray-500">{{ notesStore.notes.length }} notes</p>
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
            v-model="searchQuery"
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
          @click="showCreateForm = true"
          class="w-full bg-gray-900 hover:bg-gray-800 text-white font-medium py-2.5 px-4 rounded-md transition-colors flex items-center justify-center space-x-2 text-sm"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6"></path>
          </svg>
          <span>New Note</span>
        </button>
      </div>
    </div>

    <!-- Main Content -->
    <div class="flex-1 flex flex-col h-full overflow-hidden bg-white">
      <!-- Header -->
      <header class="bg-white border-b border-gray-200 px-8 py-6 flex-shrink-0">
        <div class="flex justify-between items-center">
          <div>
            <h1 class="text-3xl font-bold text-gray-900">My Notes</h1>
          </div>
          <div class="flex items-center space-x-3">
            <!-- Sort Controls -->
            <select
              v-model="sortBy"
              class="px-3 py-2 border border-gray-200 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent text-sm bg-white text-gray-900"
            >
              <option value="updatedAt">Recently Updated</option>
              <option value="createdAt">Recently Created</option>
              <option value="title">Title A-Z</option>
            </select>
            <button
              @click="isDescending = !isDescending"
              class="p-2 border border-gray-200 rounded-md hover:bg-gray-50 text-gray-700"
              :title="isDescending ? 'Sort Ascending' : 'Sort Descending'"
            >
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path v-if="isDescending" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 14l-7 7m0 0l-7-7m7 7V3" />
                <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 10l7-7m0 0l7 7m-7-7v18" />
              </svg>
            </button>
          </div>
        </div>
      </header>

      <!-- Content Area -->
      <div class="flex-1 overflow-y-auto px-8 py-6">
        <!-- Error Message -->
        <div v-if="notesStore.error" class="bg-red-50 border border-red-200 text-red-800 px-4 py-3 rounded-lg mb-6 flex justify-between items-center">
          <span>{{ notesStore.error }}</span>
          <button @click="notesStore.clearError()" class="text-red-600 hover:text-red-800">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
            </svg>
          </button>
        </div>

        <!-- Loading Spinner -->
        <div v-if="notesStore.loading" class="flex justify-center items-center py-20">
          <div class="animate-spin rounded-full h-8 w-8 border-2 border-gray-300 border-t-gray-900"></div>
        </div>

        <!-- Notes Grid -->
        <div v-else-if="notesStore.sortedAndFilteredNotes.length > 0" class="grid gap-4" style="grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));">
          <NoteCard
            v-for="(note, index) in notesStore.sortedAndFilteredNotes"
            :key="note.id"
            :note="note"
            :color-index="index"
            @view="viewNote"
            @edit="editNote"
            @delete="deleteNote"
            class="transform transition-all duration-150 hover:shadow-md"
          />
        </div>

        <!-- Empty State -->
        <div v-else class="text-center py-20">
          <div class="text-5xl mb-6">📝</div>
          <h3 class="text-xl font-semibold text-gray-900 mb-3">
            {{ searchQuery ? 'No matching notes' : 'Start writing' }}
          </h3>
          <p class="text-gray-600 mb-8 max-w-md mx-auto">
            {{ searchQuery 
              ? 'Try different search terms to find your notes' 
              : 'Create your first note and start organizing your thoughts' 
            }}
          </p>
          <button
            v-if="!searchQuery"
            @click="showCreateForm = true"
            class="bg-gray-900 text-white px-6 py-3 rounded-md hover:bg-gray-800 transition-colors font-medium"
          >
            Create Note
          </button>
        </div>
      </div>
    </div>

    <!-- Create/Edit Modal -->
    <NoteModal
      v-if="showCreateForm || editingNote"
      :note="editingNote"
      @save="saveNote"
      @cancel="cancelEdit"
    />

    <!-- View Modal -->
    <NoteViewModal
      v-if="viewingNote"
      :note="viewingNote"
      @close="closeView"
      @edit="editFromView"
      @request-delete="deleteFromView"
    />

    <!-- Delete Confirmation Modal -->
    <DeleteConfirmModal
      v-if="deleteConfirmNote"
      @cancel="deleteConfirmNote = null"
      @confirm="confirmDelete"
    />

    <!-- Toast Notification -->
    <Toast
      :message="toastStore.message"
      :type="toastStore.type"
      :visible="toastStore.visible"
      @close="toastStore.hideToast()"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useNotesStore } from '@/stores/notes'
import { useToastStore } from '@/stores/toast'
import type { Note, CreateNoteRequest, UpdateNoteRequest } from '@/types'
import NoteCard from './NoteCard.vue'
import NoteModal from './NoteModal.vue'
import NoteViewModal from './NoteViewModal.vue'
import DeleteConfirmModal from './DeleteConfirmModal.vue'
import Toast from './Toast.vue'

const notesStore = useNotesStore()
const toastStore = useToastStore()

// Reactive state
const showCreateForm = ref(false)
const editingNote = ref<Note | null>(null)
const viewingNote = ref<Note | null>(null)
const deleteConfirmNote = ref<Note | null>(null)
const searchQuery = ref('')
const sortBy = ref<'title' | 'createdAt' | 'updatedAt'>('updatedAt')
const isDescending = ref(true)

// Watch for changes and update store filters
watch([searchQuery, sortBy, isDescending], () => {
  notesStore.setFilters({
    search: searchQuery.value,
    sortBy: sortBy.value,
    isDescending: isDescending.value
  })
}, { immediate: true })

// Methods
const viewNote = (note: Note) => {
  viewingNote.value = note
  showCreateForm.value = false
  editingNote.value = null
}

const editNote = (note: Note) => {
  editingNote.value = note
  showCreateForm.value = false
  viewingNote.value = null
}

const deleteNote = (note: Note) => {
  deleteConfirmNote.value = note
}

const confirmDelete = async () => {
  if (!deleteConfirmNote.value) return
  try {
    await notesStore.deleteNote(deleteConfirmNote.value.id)
    toastStore.showToast('Note deleted', 'success')
    // Close view modal if we're viewing the deleted note
    if (viewingNote.value?.id === deleteConfirmNote.value.id) {
      viewingNote.value = null
    }
  } catch (error) {
    toastStore.showToast('Failed to delete note', 'error')
  } finally {
    deleteConfirmNote.value = null
  }
}

const saveNote = async (noteData: CreateNoteRequest | UpdateNoteRequest) => {
  try {
    if (editingNote.value) {
      await notesStore.updateNote(editingNote.value.id, noteData as UpdateNoteRequest)
      toastStore.showToast('Note updated', 'success')
    } else {
      await notesStore.createNote(noteData as CreateNoteRequest)
      toastStore.showToast('Note created', 'success')
    }
    cancelEdit()
  } catch (error) {
    toastStore.showToast('Failed to save note', 'error')
  }
}

const cancelEdit = () => {
  showCreateForm.value = false
  editingNote.value = null
}

const closeView = () => {
  viewingNote.value = null
}

const editFromView = (note: Note) => {
  viewingNote.value = null
  editingNote.value = note
}

const deleteFromView = (note: Note) => {
  deleteNote(note)
}

// Load notes on component mount
onMounted(() => {
  notesStore.fetchNotes()
})
</script>

<style scoped>
/* Custom scrollbar for clean look */
::-webkit-scrollbar {
  width: 6px;
}

::-webkit-scrollbar-track {
  background: #f8f9fa;
}

::-webkit-scrollbar-thumb {
  background: #e9ecef;
  border-radius: 3px;
}

::-webkit-scrollbar-thumb:hover {
  background: #dee2e6;
}
</style> 