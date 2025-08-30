import { useEffect } from "react";
import { message } from "antd";
import { useStream } from "luffie";
import {
  getMidias,
  createMidia as apiCreate,
  updateMidia as apiUpdate,
  deleteMidia as apiDelete,
} from "../api/midias";
import {
  state$,
  setMidias,
  setLoading,
  addMidiaLocal,
  updateMidiaLocal,
  removeMidiaLocal,
  setEditingMidia,
  Midia,
} from "../stores/midiasStore";

export const useMidias = () => {
  const state = useStream(state$) ?? {
    midias: [],
    loading: false,
    editingMidia: null,
  };
  const { midias, loading, editingMidia } = state;

  const loadMidias = async () => {
    setLoading(true);
    try {
      const res = await getMidias();
      setMidias(res.data);
    } catch (err) {
      console.error(err);
      message.error("Erro ao carregar mídias");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadMidias();
  }, []);

  const create = async (formData: FormData) => {
    setLoading(true);
    try {
      const res = await apiCreate(formData);
      addMidiaLocal(res.data);
      message.success("Mídia criada!");
      return res.data as Midia;
    } catch (err) {
      console.error(err);
      message.error("Erro ao criar mídia");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const update = async (id: number, formData: FormData) => {
    setLoading(true);
    try {
      const res = await apiUpdate(id, formData);
      updateMidiaLocal(res.data);
      message.success("Mídia atualizada!");
      return res.data as Midia;
    } catch (err) {
      console.error(err);
      message.error("Erro ao atualizar mídia");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const remove = async (id: number) => {
    setLoading(true);
    try {
      await apiDelete(id);
      removeMidiaLocal(id);
      message.success("Mídia deletada!");
    } catch (err) {
      console.error(err);
      message.error("Erro ao deletar mídia");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const edit = (midia: Midia | null) => {
    setEditingMidia(midia);
  };

  return {
    midias,
    loading,
    editingMidia,
    loadMidias,
    create,
    update,
    remove,
    edit,
  };
};

export default useMidias;
