<template>
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
              'text-light bg-secondary': day.date === selectedDate && day.isCurrentMonth,
              'current-day': day.date === todayDate && day.isCurrentMonth,
              'non-clickable': !day.isCurrentMonth || (!isDateBooked(day.date) && day.date < todayDate),
              subdued: day.date < todayDate && day.isCurrentMonth,
            }"
            class="day-cell"
            @click="
              () => {
                if (day.isCurrentMonth) dayClick(day.date);
              }
            "
          >
            <div class="d-flex justify-content-between">
              {{ day.day }}
              <div
                :class="{
                  'dot-booked': isDateBooked(day.date) && day.isCurrentMonth,
                }"
              ></div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
  import { computed, ref, onMounted } from "vue";
  import { VisitDto } from "@/models/VisitDto";

  const props = defineProps<{ bookedVisits?: VisitDto[]; selectedDate?: string }>();
  const emit = defineEmits(["dayClick"]);

  onMounted(() => {
    selectedDate.value = props.selectedDate ?? new Date().toISOString().split("T")[0];
  });

  const currentDate = ref(new Date());
  const selectedDate = ref<string | null>(null);
  const weekDays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

  const month = computed(() => currentDate.value.getMonth());
  const year = computed(() => currentDate.value.getFullYear());
  const monthName = computed(() => currentDate.value.toLocaleString("default", { month: "long" }));
  const todayDate = new Date().toISOString().split("T")[0];

  function dayClick(date: string) {
    selectedDate.value = date;
    emit("dayClick", date);
  }

  function isDateBooked(date: string): boolean {
    return props.bookedVisits?.some((visit) => visit.slot.split("T")[0] === date) ?? false;
  }

  function getCalendarDays() {
    const startDay = new Date(year.value, month.value, 1).getDay();

    let days = [];
    for (let i = 1 - startDay, dayCounter = 0; dayCounter < 42; i++, dayCounter++) {
      let date = new Date(year.value, month.value, i);
      date.setHours(0, 0, 0, 0);
      days.push({
        day: date.getDate(),
        date: `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, "0")}-${date.getDate().toString().padStart(2, "0")}`,
        isCurrentMonth: date.getMonth() === month.value,
        isAnyEventSet: isDateBooked(`${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, "0")}-${date.getDate().toString().padStart(2, "0")}`),
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

<style scoped>
  .day-cell {
    cursor: pointer;
  }

  .non-clickable {
    pointer-events: none;
    color: #ccc;
  }

  .subdued {
    color: #999;
  }

  .current-day {
    border: 2px solid orange;
  }

  .dot-booked {
    height: 10px;
    width: 10px;
    background-color: #0dcaf0;
    border-radius: 50%;
    display: inline-block;
  }
</style>
