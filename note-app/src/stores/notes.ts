import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Note, CreateNoteRequest, UpdateNoteRequest, NotesFilters } from '@/types'
import { NotesApi } from '@/services/api'

export const useNotesStore = defineStore('notes', () => {
  // State
  const notes = ref<Note[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const currentNote = ref<Note | null>(null)

  // Filters
  const filters = ref<NotesFilters>({
    search: '',
    sortBy: 'updatedAt',
    isDescending: true
  })

  // Getters
  const sortedAndFilteredNotes = computed(() => {
    let filtered = notes.value

    if (filters.value.search) {
      const searchTerm = filters.value.search.toLowerCase()
      filtered = filtered.filter(note => 
        note.title.toLowerCase().includes(searchTerm) ||
        note.content.toLowerCase().includes(searchTerm)
      )
    }

    return filtered.sort((a, b) => {
      const sortBy = filters.value.sortBy || 'updatedAt'
      const isDesc = filters.value.isDescending
      
      let comparison = 0
      
      if (sortBy === 'title') {
        comparison = a.title.localeCompare(b.title)
      } else if (sortBy === 'createdAt') {
        comparison = new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
      } else {
        comparison = new Date(a.updatedAt).getTime() - new Date(b.updatedAt).getTime()
      }
      
      return isDesc ? -comparison : comparison
    })
  })

  // Actions
  const fetchNotes = async () => {
    try {
      loading.value = true
      error.value = null
      notes.value = await NotesApi.getAllNotes(filters.value)
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to fetch notes'
    } finally {
      loading.value = false
    }
  }

  const fetchNoteById = async (id: number) => {
    try {
      loading.value = true
      error.value = null
      currentNote.value = await NotesApi.getNoteById(id)
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to fetch note'
    } finally {
      loading.value = false
    }
  }

  const createNote = async (noteData: CreateNoteRequest) => {
    try {
      loading.value = true
      error.value = null
      const newNote = await NotesApi.createNote(noteData)
      notes.value.unshift(newNote)
      return newNote
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to create note'
      throw err
    } finally {
      loading.value = false
    }
  }

  const updateNote = async (id: number, noteData: UpdateNoteRequest) => {
    try {
      loading.value = true
      error.value = null
      const updatedNote = await NotesApi.updateNote(id, noteData)
      const index = notes.value.findIndex(note => note.id === id)
      if (index !== -1) {
        notes.value[index] = updatedNote
      }
      if (currentNote.value?.id === id) {
        currentNote.value = updatedNote
      }
      return updatedNote
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to update note'
      throw err
    } finally {
      loading.value = false
    }
  }

  const deleteNote = async (id: number) => {
    try {
      loading.value = true
      error.value = null
      await NotesApi.deleteNote(id)
      notes.value = notes.value.filter(note => note.id !== id)
      if (currentNote.value?.id === id) {
        currentNote.value = null
      }
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to delete note'
      throw err
    } finally {
      loading.value = false
    }
  }

  const setFilters = (newFilters: Partial<NotesFilters>) => {
    filters.value = { ...filters.value, ...newFilters }
  }

  const clearError = () => {
    error.value = null
  }

  return {
    // State
    notes,
    loading,
    error,
    currentNote,
    filters,
    
    // Getters
    sortedAndFilteredNotes,
    
    // Actions
    fetchNotes,
    fetchNoteById,
    createNote,
    updateNote,
    deleteNote,
    setFilters,
    clearError
  }
}) 