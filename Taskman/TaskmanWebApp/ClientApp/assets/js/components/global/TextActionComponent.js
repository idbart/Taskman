const TextActionComponent = {
    template: `
        <div class="flex-row">
            <input type="text" v-model="text" v-bind:placeholder="placeholder"/>
            <button v-on:click="onClick">{{ btnText }}</button>
        </div>
    `,
    props: ["btnText", "placeholder"],
    emits: ["submit", "textUpdate"],
    data: function() {
        return {
            text: ""
        }
    },
    watch: {
        text: function(newText) {
            this.$emit("textUpdate", newText);
        }
    },
    methods: {
        onClick: function() {
            this.$emit("submit", this.text);
        }
    },
    computed: {

    }
};