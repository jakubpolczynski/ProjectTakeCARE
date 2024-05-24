<!-- src/components/SearchableSelect.vue -->
<template>
  <div class="searchable-select">
    <input
      type="text"
      v-model="searchQuery"
      @focus="isOpen = true"
      @blur="closeDropdown"
      @input="filterOptions"
      placeholder="Search..."
      class="form-control"
    />
    <ul
      v-if="isOpen"
      class="dropdown"
    >
      <li
        v-for="option in filteredOptions"
        :key="option.value"
        @mousedown.prevent="selectOption(option)"
        class="dropdown-item"
      >
        {{ option.label }}
      </li>
    </ul>
  </div>
</template>

<script setup>
  import { ref, watch } from "vue";

  const props = defineProps({
    options: {
      type: Array,
      required: true,
      default: () => [],
    },
    modelValue: {
      type: String,
      default: "",
    },
  });

  const emit = defineEmits(["update:modelValue"]);

  const searchQuery = ref("");
  const isOpen = ref(false);
  const filteredOptions = ref(props.options);

  const filterOptions = () => {
    if (searchQuery.value) {
      filteredOptions.value = props.options.filter((option) => option.label.toLowerCase().includes(searchQuery.value.toLowerCase()));
    } else {
      filteredOptions.value = props.options;
    }
  };

  const selectOption = (option) => {
    emit("update:modelValue", option.value);
    searchQuery.value = option.label;
    isOpen.value = false;
  };

  const closeDropdown = () => {
    setTimeout(() => {
      isOpen.value = false;
    }, 200);
  };

  watch(
    () => props.modelValue,
    (newVal) => {
      const selectedOption = props.options.find((option) => option.value === newVal);
      if (selectedOption) {
        searchQuery.value = selectedOption.label;
      }
    }
  );
</script>

<style scoped>
  .searchable-select {
    position: relative;
  }
  .search-input {
    width: 100%;
    padding: 8px;
    box-sizing: border-box;
  }
  .dropdown {
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    background: white;
    border: 1px solid #ccc;
    border-bottom-left-radius: 0.375rem;
    border-bottom-right-radius: 0.375rem;
    max-height: 200px;
    overflow-y: auto;
    z-index: 1000;
    margin: 0;
    padding: 0;
  }
  .dropdown-item {
    padding: 8px;
    cursor: pointer;
    text-align: left; /* Align text to the left */
    list-style: none; /* Remove bullet points */
  }
  .dropdown-item:hover {
    background: #f0f0f0;
  }
</style>
