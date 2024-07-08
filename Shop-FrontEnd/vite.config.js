import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  resolve: {
    plugins: [react()],
    extensions: ['.js', '.jsx', '.json', '.ts', '.tsx'],
  },
});
