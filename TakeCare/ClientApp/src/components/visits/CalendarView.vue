<template>
  <div class="container my-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <button
        class="btn btn-primary"
        @click="goToPreviousMonth"
      >
        Previous
      </button>
      <h2>{{ monthName }} {{ year }}</h2>
      <button
        class="btn btn-primary"
        @click="goToNextMonth"
      >
        Next
      </button>
    </div>

    <div class="table-responsive">
      <table class="table table-bordered">
        <thead>
          <tr>
            <th
              v-for="day in weekDays"
              :key="day"
            >
              {{ day }}
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(week, index) in calendarDays"
            :key="index"
          >
            <td
              v-for="day in week"
              :key="day.date"
              :class="{
                'bg-light': !day.isCurrentMonth,
                'text-light bg-info': day.isAnyEventSet > 8,
              }"
              @click="dayClick(day.date)"
            >
              {{ day.day }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { computed, ref } from "vue";

  const currentDate = ref(new Date());
  const weekDays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

  const month = computed(() => currentDate.value.getMonth());
  const year = computed(() => currentDate.value.getFullYear());
  const monthName = computed(() => currentDate.value.toLocaleString("default", { month: "long" }));

  function dayClick(date: Date) {
    console.log(`Clicked date: ${date}`);
    alert(`Clicked date: ${date}`);
  }

  function getCalendarDays() {
    const startDay = new Date(year.value, month.value, 1).getDay();

    let days = [];
    for (let i = 1 - startDay, dayCounter = 0; dayCounter < 42; i++, dayCounter++) {
      let date = new Date(year.value, month.value, i);
      days.push({
        day: date.getDate(),
        date: date.toISOString().split("T")[0],
        isCurrentMonth: date.getMonth() === month.value,
        isAnyEventSet: Math.floor(Math.random() * 10),
      });
    }

    return chunkArray(days, 7);
  }

  function chunkArray(array: any, size: any) {
    const chunkedArray = [];
    for (let i = 0; i < array.length; i += size) {
      chunkedArray.push(array.slice(i, i + size));
    }
    return chunkedArray;
  }

  function goToPreviousMonth() {
    const newDate = new Date(currentDate.value.getFullYear(), currentDate.value.getMonth() - 1);
    currentDate.value = newDate;
  }

  function goToNextMonth() {
    const newDate = new Date(currentDate.value.getFullYear(), currentDate.value.getMonth() + 1);
    currentDate.value = newDate;
  }

  const calendarDays = computed(getCalendarDays);
</script>
<style scoped lang="scss"></style>
