import axios from 'axios'
import type { Note, CreateNoteRequest, UpdateNoteRequest, NotesFilters } from '@/types'

const API_BASE_URL = 'http://localhost:5174/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

export class NotesApi {
  static async getAllNotes(filters?: NotesFilters): Promise<Note[]> {
    const params = new URLSearchParams()
    
    if (filters?.search) {
      params.append('search', filters.search)
    }
    if (filters?.sortBy) {
      params.append('sortBy', filters.sortBy)
    }
    if (filters?.isDescending !== undefined) {
      params.append('isDescending', filters.isDescending.toString())
    }

    const response = await api.get<Note[]>(`/notes?${params.toString()}`)
    return response.data
  }

  static async getNoteById(id: number): Promise<Note> {
    const response = await api.get<Note>(`/notes/${id}`)
    return response.data
  }

  static async createNote(note: CreateNoteRequest): Promise<Note> {
    const response = await api.post<Note>('/notes', note)
    return response.data
  }

  static async updateNote(id: number, note: UpdateNoteRequest): Promise<Note> {
    const response = await api.put<Note>(`/notes/${id}`, note)
    return response.data
  }

  static async deleteNote(id: number): Promise<void> {
    await api.delete(`/notes/${id}`)
  }
} 