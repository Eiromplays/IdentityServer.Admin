import { Spinner } from '@/components/Elements';

import { useLogin } from '../api/getLogin';
import { ExternalLoginProviders } from '../components/ExternalLoginProviders';
import { Layout } from '../components/Layout';
import { LoginForm } from '../components/LoginForm';

export const Login = () => {
  //TODO: Find a better way to get the returnUrl
  let returnUrl = '';
  const idx = location.href.toLowerCase().indexOf('?returnurl=');
  if (idx > 0) {
    returnUrl = location.href.substring(idx + 11);
  }

  const loginQuery = useLogin({ returnUrl });

  if (loginQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!loginQuery.data) return null;

  return (
    <Layout title="Log in to your account">
      <LoginForm onSuccess={() => window.location.assign('/bff/login')} />
      <ExternalLoginProviders externalProviders={loginQuery.data.visibleExternalProviders} />
    </Layout>
  );
};