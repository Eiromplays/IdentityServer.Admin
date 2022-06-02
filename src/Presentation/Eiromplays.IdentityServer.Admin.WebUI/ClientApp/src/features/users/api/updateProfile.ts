import { axios, MutationConfig, useAuth } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type UpdateProfileDTO = {
  userId: string;
  data: {
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    gravatarEmail: string;
    image: any;
    deleteCurrentImage?: boolean;
  };
};

// even if you decide not to use precaching. See https://cra.link/PWA
const toBase64 = (file: File) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

export const updateProfile = async ({ userId, data }: UpdateProfileDTO) => {
  if (data.image instanceof File) {
    const fileExtension = `.${data.image.name.slice(
      ((data.image.name.lastIndexOf('.') - 1) >>> 0) + 2
    )}`;
    data.image = {
      data: await toBase64(data.image),
      extension: fileExtension,
      name: data.image.name.replace(fileExtension, ''),
    };
    data.deleteCurrentImage = data.image ? true : data.deleteCurrentImage;
  }

  return axios.put(`/users/${userId}`, data);
};

type UseUpdateProfileOptions = {
  config?: MutationConfig<typeof updateProfile>;
};

export const useUpdateProfile = ({ config }: UseUpdateProfileOptions = {}) => {
  const { refetchUser } = useAuth();

  const updateProfileMutation = useMutation({
    onSuccess: async () => {
      toast.success('User Updated');
      await refetchUser();
    },
    onError: (error) => {
      toast.error('Failed to update user');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateProfile,
  });

  return { updateProfileMutation };
};
