import { createStore } from "luffie";

export interface Midia {
  id: number;
  nome: string;
  descricao: string;
  urlMidia: string;
}

export interface MidiasState {
  midias: Midia[];
  loading: boolean;
  editingMidia: Midia | null;
}

const initialState: MidiasState = {
  midias: [],
  loading: false,
  editingMidia: null,
};

export const { state$, updateState } = createStore<MidiasState>(initialState);

export const setMidias = (midias: Midia[]) =>
  updateState((prev: MidiasState) => ({ ...prev, midias }));

export const setLoading = (loading: boolean) =>
  updateState((prev: MidiasState) => ({ ...prev, loading }));

export const setEditingMidia = (midia: Midia | null) =>
  updateState((prev: MidiasState) => ({ ...prev, editingMidia: midia }));

export const addMidiaLocal = (midia: Midia) =>
  updateState((prev: MidiasState) => ({
    ...prev,
    midias: [...prev.midias, midia],
  }));

export const updateMidiaLocal = (midia: Midia) =>
  updateState((prev: MidiasState) => ({
    ...prev,
    midias: prev.midias.map((m) => (m.id === midia.id ? midia : m)),
    editingMidia:
      prev.editingMidia && prev.editingMidia.id === midia.id
        ? midia
        : prev.editingMidia,
  }));

export const removeMidiaLocal = (id: number) =>
  updateState((prev: MidiasState) => ({
    ...prev,
    midias: prev.midias.filter((m) => m.id !== id),
    editingMidia:
      prev.editingMidia && prev.editingMidia.id === id
        ? null
        : prev.editingMidia,
  }));
